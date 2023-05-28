using System.Text.Json;
using Moq;
using BlockchainLib.Cryptography;
using HighLevelBlockchain;
using BlockChainApp;

namespace BlockchainAppTests.BlockchainAppTests;

public class SimpleAppTests
{
    private const string FromKey = "FromKey";
    private const string ToKey = "ToKey";
    private const string Property = "Property";
    private const string Signature = "Signature";
    private Keys _keys;

    private Mock<IGenericBlockchain<PropertyTransactionBlock>> _mockBlockchain;
    private Mock<IEncryptorService> _mockEncryptorService;
    private SimpleApp _simpleApp;

    [SetUp]
    public void Setup()
    {
        _keys = new Keys(FromKey, "PrivateKey");
        _mockBlockchain = new Mock<IGenericBlockchain<PropertyTransactionBlock>>();
        _mockEncryptorService = new Mock<IEncryptorService>();
        _simpleApp = new SimpleApp(_mockEncryptorService.Object, _mockBlockchain.Object);
    }

    [Test]
    public void GenerateKeys_ReturnsKeysFromEncryptorService()
    {
        var expectedKeys = new Keys("publicKey", "privateKey");
        _mockEncryptorService.Setup(es => es.GenerateKeyPair()).Returns(expectedKeys);

        var result = _simpleApp.GenerateKeys();

        Assert.AreEqual(expectedKeys, result);
    }

    [Test]
    public void PerformTransaction_BuildsAndProcessesBlock()
    {
        var transactionBlock = new PropertyTransactionBlock(
            new PropertyTransaction(FromKey, ToKey, Property, DateTime.Now), Signature);
        var genericBlock = new GenericBlock<PropertyTransactionBlock>(
            "hash", "parentHash", JsonSerializer.Serialize(transactionBlock), transactionBlock);
        
        _mockEncryptorService.Setup(es => es.SignData(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Signature)
            .Callback((string data, string key) =>
            {
                Console.WriteLine($"SignData called with data: {data}, key: {key}");
            });

        _mockBlockchain
            .Setup(bc => bc.BuildBlock(It.Is<PropertyTransactionBlock>(ptb =>
                ptb.TransactionPayload.From == FromKey &&
                ptb.TransactionPayload.To == ToKey &&
                ptb.TransactionPayload.Property == Property &&
                ptb.Sign == Signature)))
            .Returns(genericBlock);

        _simpleApp.PerformTransaction(_keys, ToKey, Property);
        
        _mockBlockchain.Verify(bc => bc.BuildBlock(It.IsAny<PropertyTransactionBlock>()), Times.Once);
        _mockBlockchain.Verify(bc => bc.ProcessBlock(It.IsAny<GenericBlock<PropertyTransactionBlock>>()), Times.Once);
        _mockEncryptorService.Verify(es => es.SignData(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
    
    [Test]
    public void PerformTransaction_CallsBuildAndProcessBlockOnce_WhenValidDataIsPassed()
    {
        // Arrange
        var fromKeys = new Keys("PublicKey", "PrivateKey");
        var to = "ToKey";
        var property = "Property";
        var Signature = "Signature";

        var mockEncryptorService = new Mock<IEncryptorService>();
        mockEncryptorService.Setup(es => 
            es.SignData(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Signature);

        var mockBlockchain = new Mock<IGenericBlockchain<PropertyTransactionBlock>>();
        mockBlockchain.Setup(bc => 
            bc.BuildBlock(It.IsAny<PropertyTransactionBlock>()))
            .Returns(
                new GenericBlock<PropertyTransactionBlock>("Hash", "ParentHash", "RawData", 
                new PropertyTransactionBlock(new PropertyTransaction("FromKey", "ToKey", "Property", DateTime.Now),
                    "Signature")));

        var simpleApp = new SimpleApp(mockEncryptorService.Object, mockBlockchain.Object);

        // Act
        simpleApp.PerformTransaction(fromKeys, to, property);

        // Assert
        mockBlockchain.Verify(
            bc => 
                bc.BuildBlock(It.Is<PropertyTransactionBlock>(ptb =>
                ptb.TransactionPayload.From == fromKeys.Public && ptb.TransactionPayload.To == to &&
                ptb.TransactionPayload.Property == property && ptb.Sign == Signature)), Times.Once);
        
        mockBlockchain.Verify(bc => 
            bc.ProcessBlock(It.IsAny<GenericBlock<PropertyTransactionBlock>>()), Times.Once);
    }

}
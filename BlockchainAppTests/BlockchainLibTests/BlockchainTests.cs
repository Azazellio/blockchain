using BlockchainLib;
using BlockchainLib.BlockchainServices;
using BlockchainLib.Hash;
using Moq;

namespace BlockchainAppTests.BlockchainLibTests;

[TestFixture]
public class BlockchainTests
{
    private const string _data = "data";
    private const string _wrongHash = "wrong hash";
    private const string _moreData = "more data";

    private Mock<IHashFunction> _mockHashFunction;
    private Blockchain _blockchain;

    [SetUp]
    public void SetUp()
    {
        // Set up a mock hash function that returns the input string as the hash.
        _mockHashFunction = new Mock<IHashFunction>();
        _mockHashFunction.Setup(hf => hf.Hash(It.IsAny<string>())).Returns((string s) => s);

        _blockchain = new Blockchain(_mockHashFunction.Object);
    }

    [Test]
    public void AddBlock_FirstBlock_AddsBlock()
    {
        // Arrange
        var block = new Block(null, _data, _data);

        // Act
        _blockchain.AddBlock(block);

        // Assert
        Assert.AreEqual(block, _blockchain.Single());
    }

    [Test]
    public void AddBlock_ValidBlock_AddsBlock()
    {
        // Arrange
        var firstBlock = new Block(null, _data, _data);
        var secondBlock = new Block(_data, _moreData, _data + _moreData);

        // Act
        _blockchain.AddBlock(firstBlock);
        _blockchain.AddBlock(secondBlock);

        // Assert
        Assert.AreEqual(2, _blockchain.Count());
        Assert.AreEqual(secondBlock, _blockchain.Last());
    }

    [Test]
    public void AddBlock_InvalidParentHash_ThrowsException()
    {
        // Arrange
        var firstBlock = new Block(null, _data, _data);
        var secondBlock = new Block(_wrongHash, _moreData, _wrongHash + _moreData);

        _blockchain.AddBlock(firstBlock);

        // Act & Assert
        Assert.Throws<ApplicationException>(() => _blockchain.AddBlock(secondBlock));
    }
}

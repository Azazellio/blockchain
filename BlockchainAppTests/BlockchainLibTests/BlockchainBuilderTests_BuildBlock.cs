using BlockchainLib.BlockchainServices.BlockchainBuilder;
using BlockchainLib.Hash;
using Moq;

namespace BlockchainAppTests.BlockchainLibTests;

[TestFixture]
public class BlockchainBuilderTests_BuildBlock
{
    private Mock<IHashFunction> _mockHashFunction;
    private BlockchainBuilder _blockchainBuilder;

    private const string _lastBlockHash = "lastBlockHash";
    private const string _mockHash = "mockHash";
    private const string _data = "data";

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _mockHashFunction = new Mock<IHashFunction>();
        _mockHashFunction.Setup(hashFunction => hashFunction.Hash(It.IsAny<string>())).Returns(_mockHash);

        _blockchainBuilder = new BlockchainBuilder(_mockHashFunction.Object, _lastBlockHash);
    }

    [Test]
    public void BuildBlock_WhenCalled_ReturnsBlockWithCorrectHash()
    {
        // Act
        var block = _blockchainBuilder.BuildBlock(_data);

        // Assert
        Assert.AreEqual(_mockHash, block.Hash);
    }

    [Test]
    public void BuildBlock_WhenCalled_ReturnsBlockWithCorrectData()
    {
        // Act
        var block = _blockchainBuilder.BuildBlock(_data);

        // Assert
        Assert.AreEqual(_data, block.Data);
    }

    [Test]
    public void BuildBlock_WhenCalled_ReturnsBlockWithCorrectParentHash()
    {
        // Act
        var block = _blockchainBuilder.BuildBlock(_data);

        // Assert
        Assert.AreEqual(_lastBlockHash, block.ParentHash);
    }

    [Test]
    public void BuildBlock_WhenCalled_CallsHashFunctionWithCorrectInput()
    {
        // Act
        _blockchainBuilder.BuildBlock(_data);

        // Assert
        _mockHashFunction.Verify(hashFunction => hashFunction.Hash(_lastBlockHash + _data), Times.Once);
    }
}

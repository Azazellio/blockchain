using BlockchainLib.BlockchainServices.BlockchainBuilder;
using BlockchainLib.Hash;
using Moq;

namespace BlockchainAppTests.BlockchainLibTests;

[TestFixture]
public class BlockchainBuilderTests_ProcessBlock
{
    private BlockchainBuilder _blockchainBuilder;
    private Mock<IHashFunction> _hashFunctionMock;
    private const string initialBlockData = "Genesis Block";
    private const string newBlockData = "New Block";
    private const string initialBlockHash = "1234567890";
    private const string newBlockHash = "0987654321";

    [SetUp]
    public void Setup()
    {
        _hashFunctionMock = new Mock<IHashFunction>();
        _hashFunctionMock.Setup(h => h.Hash(initialBlockData)).Returns(initialBlockHash);
        _hashFunctionMock.Setup(h => h.Hash(newBlockData)).Returns(newBlockHash);
        
        _blockchainBuilder = new BlockchainBuilder(_hashFunctionMock.Object, null);
    }

    [Test]
    public void BuildBlock_ShouldReturnValidBlock()
    {
        var block = _blockchainBuilder.BuildBlock(initialBlockData);

        Assert.AreEqual(initialBlockData, block.Data);
        Assert.AreEqual(initialBlockHash, block.Hash);
        Assert.IsNull(block.ParentHash);
    }

    [Test]
    public void ProcessBlock_ShouldUpdateLastBlockHash()
    {
        var block = _blockchainBuilder.BuildBlock(initialBlockData);
        _blockchainBuilder.ProcessBlock(block);
        
        var newBlock = _blockchainBuilder.BuildBlock(newBlockData);
        
        Assert.AreEqual(block.Hash, newBlock.ParentHash);
    }
}
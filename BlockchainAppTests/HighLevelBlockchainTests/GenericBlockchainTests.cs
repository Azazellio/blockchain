using BlockchainLib;
using BlockchainLib.BlockchainServices;
using BlockchainLib.BlockchainServices.BlockchainBuilder;
using HighLevelBlockchain;
using HighLevelBlockchain.Indexes;
using HighLevelBlockchain.Rules.Abstractions;
using Moq;

namespace BlockchainAppTests.HighLevelBlockchainTests;

[TestFixture]
public class GenericBlockchainTests
{
    private const string TestData = "Test Data";
    private const string TestHash = "Test Hash";
    private const string TestParentHash = "Test Parent Hash";

    private Mock<IBlockchainBuilder> _blockchainBuilderMock;
    private Mock<IBlockchain> _blockchainMock;
    private IRule<string>[] _rules;
    private IIndexBuilder<string>[] _indexes;

    [SetUp]
    public void SetUp()
    {
        _blockchainBuilderMock = new Mock<IBlockchainBuilder>();
        _blockchainMock = new Mock<IBlockchain>();
        
        _rules = new IRule<string>[0];
        _indexes = new IIndexBuilder<string>[0];
    }

    [Test]
    public void BuildBlock_ShouldReturnGenericBlock_WithExpectedData()
    {
        // Arrange
        _blockchainBuilderMock.Setup(x => x.BuildBlock(It.IsAny<string>()))
            .Returns(new Block(TestParentHash, TestData, TestHash));

        var sut = new GenericBlockchain<string>(_blockchainBuilderMock.Object, _blockchainMock.Object, _rules, _indexes);

        // Act
        var result = sut.BuildBlock(TestData);

        // Assert
        Assert.AreEqual(TestData, result.Data);
        Assert.AreEqual(TestHash, result.Hash);
        Assert.AreEqual(TestParentHash, result.ParentHash);
    }

    [Test]
    public void ProcessBlock_ShouldAddBlockToBlockchain_AndProcessRulesAndIndexes()
    {
        // Arrange
        var genericBlock = new GenericBlock<string>(TestHash, TestParentHash, TestData, TestData);

        var ruleMock = new Mock<IRule<string>>();
        var indexMock = new Mock<IIndexBuilder<string>>();
        _rules = new[] { ruleMock.Object };
        _indexes = new[] { indexMock.Object };

        var sut = new GenericBlockchain<string>(_blockchainBuilderMock.Object, _blockchainMock.Object, _rules, _indexes);

        // Act
        sut.ProcessBlock(genericBlock);

        // Assert
        _blockchainMock.Verify(x => x.AddBlock(It.IsAny<Block>()), Times.Once);
        _blockchainBuilderMock.Verify(x => x.ProcessBlock(It.IsAny<Block>()), Times.Once);
        ruleMock.Verify(x => x.Execute(sut, genericBlock), Times.Once);
        indexMock.Verify(x => x.IndexBlock(genericBlock.Data), Times.Once);
    }

    [Test]
    public void GetEnumerator_ShouldReturnExpectedGenericBlocks()
    {
        // Arrange
        var block = new Block(TestParentHash, $"\"{TestData}\"", TestHash);
        var blockList = new List<Block> { block };
        _blockchainMock.Setup(x => x.GetEnumerator()).Returns(blockList.GetEnumerator());

        var sut = new GenericBlockchain<string>(_blockchainBuilderMock.Object, _blockchainMock.Object, _rules, _indexes);

        // Act
        var result = sut.ToList();

        // Assert
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(TestData, result[0].Data);
        Assert.AreEqual(TestHash, result[0].Hash);
        Assert.AreEqual(TestParentHash, result[0].ParentHash);
    }
}
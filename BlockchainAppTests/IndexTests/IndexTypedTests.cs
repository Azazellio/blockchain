using HighLevelBlockchain.Indexes;

namespace BlockchainAppTests.IndexTests;

public class IndexTypedTests
{
    const string KEY1 = "Key1";
    const string KEY2 = "Key2";
    const string DATA1 = "Data1";
    const string DATA2 = "Data2";

    public class TestBlock
    {
        public string Key { get; set; }
        public string Data { get; set; }
    }

    [Test]
    public void IndexTyped_IndexBlock_WithNewKey_ShouldIndexCorrectly()
    {
        // Arrange
        var indexTyped = new IndexTyped<TestBlock, string>(block => block.Key);

        // Act
        indexTyped.IndexBlock(new TestBlock { Key = KEY1, Data = DATA1 });

        // Assert
        var blocks = indexTyped.GetByKey(KEY1).ToList();
        Assert.AreEqual(1, blocks.Count);
        Assert.AreEqual(KEY1, blocks[0].Key);
        Assert.AreEqual(DATA1, blocks[0].Data);
    }

    [Test]
    public void IndexTyped_IndexBlock_WithExistingKey_ShouldAddToExistingIndex()
    {
        // Arrange
        var indexTyped = new IndexTyped<TestBlock, string>(block => block.Key);
        indexTyped.IndexBlock(new TestBlock { Key = KEY1, Data = DATA1 });

        // Act
        indexTyped.IndexBlock(new TestBlock { Key = KEY1, Data = DATA2 });

        // Assert
        var blocks = indexTyped.GetByKey(KEY1).ToList();
        Assert.AreEqual(2, blocks.Count);
        Assert.AreEqual(KEY1, blocks[0].Key);
        Assert.AreEqual(DATA1, blocks[0].Data);
        Assert.AreEqual(KEY1, blocks[1].Key);
        Assert.AreEqual(DATA2, blocks[1].Data);
    }

    [Test]
    public void IndexTyped_GetByKey_WithNonexistentKey_ShouldReturnEmptyEnumerable()
    {
        // Arrange
        var indexTyped = new IndexTyped<TestBlock, string>(block => block.Key);

        // Act
        var blocks = indexTyped.GetByKey(KEY2);

        // Assert
        Assert.IsEmpty(blocks);
    }

    [Test]
    public void IndexTyped_IndexBlock_WithDifferentKeys_ShouldIndexCorrectly()
    {
        // Arrange
        var indexTyped = new IndexTyped<TestBlock, string>(block => block.Key);
        indexTyped.IndexBlock(new TestBlock { Key = KEY1, Data = DATA1 });

        // Act
        indexTyped.IndexBlock(new TestBlock { Key = KEY2, Data = DATA2 });

        // Assert
        var blocksKey1 = indexTyped.GetByKey(KEY1).ToList();
        var blocksKey2 = indexTyped.GetByKey(KEY2).ToList();
        Assert.AreEqual(1, blocksKey1.Count);
        Assert.AreEqual(1, blocksKey2.Count);
        Assert.AreEqual(KEY1, blocksKey1[0].Key);
        Assert.AreEqual(DATA1, blocksKey1[0].Data);
        Assert.AreEqual(KEY2, blocksKey2[0].Key);
        Assert.AreEqual(DATA2, blocksKey2[0].Data);
    }
}
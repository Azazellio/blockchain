using BlockChainApp;
using BlockChainApp.CustomIndexes;

namespace BlockchainAppTests.BlockchainAppTests.CustomIndexes;

public class PropertyIndexTests
{
    private PropertyIndex _index;
    private const string PropertyName1 = "Property1";
    private const string PropertyName2 = "Property2";
    private PropertyTransactionBlock _block1;
    private PropertyTransactionBlock _block2;

    [SetUp]
    public void Setup()
    {
        _index = new PropertyIndex();

        _block1 = new PropertyTransactionBlock(
            new PropertyTransaction("From1", "To1", PropertyName1, DateTime.Now), 
            "Sign1"
        );

        _block2 = new PropertyTransactionBlock(
            new PropertyTransaction("From2", "To2", PropertyName2, DateTime.Now), 
            "Sign2"
        );
    }

    [Test]
    public void IndexBlock_ShouldIndexCorrectly()
    {
        _index.IndexBlock(_block1);
        _index.IndexBlock(_block2);

        var results1 = _index.GetByKey(PropertyName1);
        var results2 = _index.GetByKey(PropertyName2);

        Assert.Contains(_block1, results1.ToList());
        Assert.Contains(_block2, results2.ToList());
    }

    [Test]
    public void IndexBlock_ShouldHandleMultipleBlocksForSameProperty()
    {
        _index.IndexBlock(_block1);
        _index.IndexBlock(_block1);

        var results = _index.GetByKey(PropertyName1);

        Assert.AreEqual(2, results.Count());
    }

    [Test]
    public void GetByKey_ShouldReturnEmptyIfNoMatches()
    {
        var results = _index.GetByKey("NonExistentProperty");

        Assert.IsEmpty(results);
    }
}
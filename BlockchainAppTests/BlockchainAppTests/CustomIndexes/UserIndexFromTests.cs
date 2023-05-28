using BlockChainApp;
using BlockChainApp.CustomIndexes;

namespace BlockchainAppTests.BlockchainAppTests.CustomIndexes;

public class UserIndexFromTests
{
    private UserIndexFrom _index;
    private const string UserFrom1 = "User1";
    private const string UserFrom2 = "User2";
    private PropertyTransactionBlock _block1;
    private PropertyTransactionBlock _block2;

    [SetUp]
    public void Setup()
    {
        _index = new UserIndexFrom();

        _block1 = new PropertyTransactionBlock(
            new PropertyTransaction(UserFrom1, "To1", "Property1", DateTime.Now), 
            "Sign1"
        );

        _block2 = new PropertyTransactionBlock(
            new PropertyTransaction(UserFrom2, "To2", "Property2", DateTime.Now), 
            "Sign2"
        );
    }

    [Test]
    public void IndexBlock_ShouldIndexCorrectly()
    {
        _index.IndexBlock(_block1);
        _index.IndexBlock(_block2);

        var results1 = _index.GetByKey(UserFrom1);
        var results2 = _index.GetByKey(UserFrom2);

        Assert.Contains(_block1, results1.ToList());
        Assert.Contains(_block2, results2.ToList());
    }

    [Test]
    public void IndexBlock_ShouldHandleMultipleBlocksForSameUser()
    {
        _index.IndexBlock(_block1);
        _index.IndexBlock(_block1);

        var results = _index.GetByKey(UserFrom1);

        Assert.AreEqual(2, results.Count());
    }

    [Test]
    public void GetByKey_ShouldReturnEmptyIfNoMatches()
    {
        var results = _index.GetByKey("NonExistentUser");

        Assert.IsEmpty(results);
    }
}
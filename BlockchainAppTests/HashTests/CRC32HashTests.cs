using BlockchainLib.Hash;

namespace BlockchainAppTests.HashTests;

[TestFixture]
public class CRC32HashTests
{
    private CRC32Hash _crc32Hash;

    [SetUp]
    public void Setup()
    {
        _crc32Hash = new CRC32Hash();
    }

    [Test]
    [TestCase("Hello, World!", "3iIiFG")]
    [TestCase("Test data", "1H_ImH")]
    public void Hash_ShouldReturnExpectedHash_WhenDataIsProvided(string input, string expected)
    {
        var result = _crc32Hash.Hash(input);

        Assert.AreEqual(expected, result);
    }

    [Test]
    public void Hash_ShouldReturnDifferentHashes_WhenDifferentDataIsProvided()
    {
        var data1 = "Test data 1";
        var data2 = "Test data 2";

        var result1 = _crc32Hash.Hash(data1);
        var result2 = _crc32Hash.Hash(data2);

        Assert.AreNotEqual(result1, result2);
    }

    [Test]
    public void Hash_ShouldReturnSameHash_WhenSameDataIsProvided()
    {
        var data = "Test data";

        var result1 = _crc32Hash.Hash(data);
        var result2 = _crc32Hash.Hash(data);

        Assert.AreEqual(result1, result2);
    }
}
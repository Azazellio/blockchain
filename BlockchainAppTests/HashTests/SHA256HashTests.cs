using BlockchainLib.Hash;

namespace BlockchainAppTests.HashTests;

[TestFixture]
public class SHA256HashTests
{
    private SHA256Hash _sha256Hash;

    [SetUp]
    public void SetUp()
    {
        _sha256Hash = new SHA256Hash();
    }

    [Test]
    [TestCase("Hello, World!", "dffd6021bb2bd5b0af676290809ec3a53191dd81c7f70a4b28688a362182986f")]
    [TestCase("Blockchain", "625da44e4eaf58d61cf048d168aa6f5e492dea166d8bb54ec06c30de07db57e1")]
    public void Hash_CorrectInput_ReturnsExpectedHash(string input, string expected)
    {
        // Act
        var hash = _sha256Hash.Hash(input);

        // Assert
        Assert.AreEqual(expected, hash);
    }
}

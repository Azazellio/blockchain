using BlockchainLib.Cryptography;
using NUnit.Framework;
namespace BlockchainAppTests;

[TestFixture]
public class RSAEncryptorTests
{
    private RSAEncryptor _rsaEncryptor;
    private const string TESTDATA = "Hello, World!";
    private Keys _keys;

    [SetUp]
    public void Setup()
    {
        _rsaEncryptor = new RSAEncryptor();
        _keys = _rsaEncryptor.GenerateKeyPair();
    }

    [Test]
    public void GenerateKeyPair_ShouldReturnNonNullKeys()
    {
        Assert.IsNotNull(_keys.Public);
        Assert.IsNotNull(_keys.Private);
    }

    [Test]
    public void SignData_ShouldReturnNonNullSignature()
    {
        var signature = _rsaEncryptor.SignData(TESTDATA, _keys.Private);

        Assert.IsNotNull(signature);
    }

    [Test]
    public void VerifySignature_ShouldReturnTrueForValidSignature()
    {
        var signature = _rsaEncryptor.SignData(TESTDATA, _keys.Private);
        var isValid = _rsaEncryptor.VerifySignature(_keys.Public, TESTDATA, signature);

        Assert.IsTrue(isValid);
    }

    [Test]
    public void Encrypt_ShouldReturnNonNullCipherText()
    {
        var cipherText = _rsaEncryptor.Encrypt(_keys.Public, TESTDATA);

        Assert.IsNotNull(cipherText);
    }

    [Test]
    public void Decrypt_ShouldReturnOriginalPlainText()
    {
        var cipherText = _rsaEncryptor.Encrypt(_keys.Public, TESTDATA);
        var decryptedText = _rsaEncryptor.Decrypt(_keys.Private, cipherText);

        Assert.AreEqual(TESTDATA, decryptedText);
    }
}

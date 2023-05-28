using BlockchainLib.Cryptography;
using HighLevelBlockchain;
using HighLevelBlockchain.Rules;
using Moq;

namespace BlockchainAppTests.Rules;

[TestFixture]
public class SignRuleTests
{
    private Mock<IEncryptorService> _encryptorService;
    private SignRule<ISignedBlock<string>, string> _signRule;

    [SetUp]
    public void Setup()
    {
        _encryptorService = new Mock<IEncryptorService>();
        _signRule = new SignRule<ISignedBlock<string>, string>(_encryptorService.Object);
    }

    [Test]
    public void Execute_ValidSignature_DoesNotThrow()
    {
        // Arrange
        const string publicKey = "publicKey";
        const string sign = "sign";
        const string data = "data";
        var signedBlock = new Mock<ISignedBlock<string>>();
        signedBlock.Setup(s => s.PublicKey).Returns(publicKey);
        signedBlock.Setup(s => s.Sign).Returns(sign);
        signedBlock.Setup(s => s.Data).Returns(data);
        var block = new GenericBlock<ISignedBlock<string>>(null, null, null, signedBlock.Object);
        _encryptorService.Setup(e => e.VerifySignature(publicKey, It.IsAny<string>(), sign)).Returns(true);

        // Act & Assert
        Assert.DoesNotThrow(() => _signRule.Execute(new List<GenericBlock<ISignedBlock<string>>>(), block));
    }

    [Test]
    public void Execute_InvalidSignature_ThrowsApplicationException()
    {
        // Arrange
        const string publicKey = "publicKey";
        const string sign = "sign";
        const string data = "data";
        var signedBlock = new Mock<ISignedBlock<string>>();
        signedBlock.Setup(s => s.PublicKey).Returns(publicKey);
        signedBlock.Setup(s => s.Sign).Returns(sign);
        signedBlock.Setup(s => s.Data).Returns(data);
        var block = new GenericBlock<ISignedBlock<string>>(null, null, null, signedBlock.Object);
        _encryptorService.Setup(e => e.VerifySignature(publicKey, It.IsAny<string>(), sign)).Returns(false);

        // Act & Assert
        Assert.Throws<ApplicationException>(() => _signRule.Execute(new List<GenericBlock<ISignedBlock<string>>>(), block));
    }
}

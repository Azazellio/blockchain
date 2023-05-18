namespace BlockchainLib.Cryptography;

public interface IEncryptorService
{
    KeyPair GenerateKeys();
    string Sign(string data, string privateKey);
    bool VerifySign(string publicKey, string data, string sign);
}
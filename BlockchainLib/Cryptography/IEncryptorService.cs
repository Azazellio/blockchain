namespace BlockchainLib.Cryptography;

public interface IEncryptorService
{
    Keys GenerateKeyPair();
    string SignData(string data, string privateKey);
    bool VerifySignature(string publicKey, string data, string sign);
}
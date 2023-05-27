using System.Security.Cryptography;
using System.Text;
using System;

namespace BlockchainLib.Cryptography;

public class RSACrossPlatformEncryptor : IEncryptorService
{
    public Keys GenerateKeyPair()
    {
        using var rsa = RSA.Create();
        var @private = rsa.ExportParameters(true);
        var @public = rsa.ExportParameters(false);
        var publicKey = Convert.ToBase64String(GetArray(@public));
        var privateKey = Convert.ToBase64String(GetArray(@private));

        return new Keys(publicKey, privateKey);
    }

    public string SignData(string data, string privateKey)
    {
        using var provider = GetCryptoProvider(privateKey);
        var bytes = Encoding.UTF8.GetBytes(data);
        var signedHash = provider.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signedHash);
    }

    public bool VerifySignature(string publicKey, string data, string sign)
    {
        using var provider = GetVerificationProvider(publicKey);
        var signBytes = Convert.FromBase64String(sign);
        var dataBytes = Encoding.UTF8.GetBytes(data);
        return provider.VerifyData(dataBytes, signBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        // public bool VerifyData(byte[] data, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
    }

    public string Encrypt(string publicKey, string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        using var cryptoProvider = GetVerificationProvider(publicKey);
        return Convert.ToBase64String(cryptoProvider.Encrypt(bytes, RSAEncryptionPadding.Pkcs1));
    }

    public string Decrypt(string privateKey, string data)
    {
        using var provider = GetCryptoProvider(privateKey);
        var bytes = Convert.FromBase64String(data);
        var decrypted = provider.Decrypt(bytes, RSAEncryptionPadding.Pkcs1);
        return Encoding.UTF8.GetString(decrypted);
    }

    private static RSA GetVerificationProvider(string publicKey)
    {
        var parameter = GetPublicKey(Convert.FromBase64String(publicKey));
        var provider = RSA.Create();
        provider.ImportParameters(parameter);
        return provider;
    }

    private static RSA GetCryptoProvider(string privateKey)
    {
        var privateParameter = GetPrivateKey(Convert.FromBase64String(privateKey));
        var provider = RSA.Create();
        provider.ImportParameters(privateParameter);

        return provider;
    }

    // Your GetPublicKey, GetPrivateKey, FillArray, GetArray, GetLength, CopyTo methods are left as is.
    
    private static RSAParameters GetPublicKey(byte[] byteRepresentation)
    {
        int pos = 0;
        var result = new RSAParameters();
        result.Exponent = FillArray(3, byteRepresentation, ref pos);
        result.Modulus = FillArray(128, byteRepresentation, ref pos);
        return result;
    }

    private static RSAParameters GetPrivateKey(byte[] byteRepresentation)
    {
        int pos = 0;
        var result = new RSAParameters();
        result.D = FillArray(128, byteRepresentation, ref pos);
        result.DP = FillArray(64, byteRepresentation, ref pos);
        result.DQ = FillArray(64, byteRepresentation, ref pos);
        result.Exponent = FillArray(3, byteRepresentation, ref pos);
        result.InverseQ = FillArray(64, byteRepresentation, ref pos);
        result.Modulus = FillArray(128, byteRepresentation, ref pos);
        result.P = FillArray(64, byteRepresentation, ref pos);
        result.Q = FillArray(64, byteRepresentation, ref pos);
        return result;
    }

    private static byte[] FillArray(int length, byte[] byteRepresentation, ref int pos)
    {
        var result = new byte[length];
        Array.Copy(byteRepresentation, pos, result, 0, length);
        pos += length;
        return result;
    }

    private static byte[] GetArray(RSAParameters p)
    {
        var length =
              GetLength(p.D)
            + GetLength(p.DP)
            + GetLength(p.DQ)
            + GetLength(p.Exponent)
            + GetLength(p.InverseQ)
            + GetLength(p.Modulus)
            + GetLength(p.P)
            + GetLength(p.Q);
        length = (length / 4 + 1) * 4;
        byte[] data = new byte[length];
        var pos = 0;
        pos = CopyTo(data, p.D, pos);
        pos = CopyTo(data, p.DP, pos);
        pos = CopyTo(data, p.DQ, pos);
        pos = CopyTo(data, p.Exponent, pos);
        pos = CopyTo(data, p.InverseQ, pos);
        pos = CopyTo(data, p.Modulus, pos);
        pos = CopyTo(data, p.P, pos);
        CopyTo(data, p.Q, pos);

        return data;
    }

    private static int GetLength(byte[]? d) => d?.Length ?? 0;

    private static int CopyTo(byte[] buffer, byte[]? dataToCopy, int pos)
    {
        if (dataToCopy != null)
        {
            dataToCopy.CopyTo(buffer, pos);
            return pos + dataToCopy.Length;
        }
        return pos;
    }
}

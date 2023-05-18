using System.Security.Cryptography;
using System.Text;

namespace BlockchainLib.Hash;

public class SHA256Hash : IHashFunction
{
    public string Hash(string data)
    {
        var sha = SHA256.HashData(Encoding.UTF8.GetBytes(data));
        return string.Concat(sha.Select(x => $"{x:x2}"));
    }
}
using BlockchainLib.Hash;

namespace BlockchainLib;

public record Block(string ParentHash, string Data, string Hash)
{
    public string GetHash(IHashFunction hashFunction)
    {
        return hashFunction.Hash(ParentHash + Data);
    }
}
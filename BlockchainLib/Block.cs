using BlockchainLib.Hash;

namespace BlockchainLib;

public record Block(string ParentHash, string Data, string Hash)
{
    public string GetBlockHash(IHashFunction hashFunction)
    {
        return hashFunction.Hash(ParentHash + Data);
    }
}

// public interface IBlockEntity
// {
//     string ParentHash { get; }
//     string Data { get; }
//     string Hash { get; }
//
//     string GetBlockHash(IHashFunction hashFunction)
//     {
//         return hashFunction.Hash(ParentHash + Data);
//     }
// }

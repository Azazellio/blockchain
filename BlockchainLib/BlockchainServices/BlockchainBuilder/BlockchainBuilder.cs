using BlockchainLib.Hash;

namespace BlockchainLib.BlockchainServices.BlockchainBuilder;

public class BlockchainBuilder : IBlockchainBuilder
{
    private readonly IHashFunction _hashFunction;
    private string? _lastBlockHash;
    
    public BlockchainBuilder(
        IHashFunction hashFunction,
        string? lastBlockHash)
    {
        _hashFunction = hashFunction;
        _lastBlockHash = lastBlockHash;
    }

    public Block BuildBlock(string data)
    {
        var hash = _hashFunction.Hash(_lastBlockHash + data);
        var block = new Block(_lastBlockHash!, data, hash);
        return block;
    }

    public void ProcessBlock(Block block)
    {
        _lastBlockHash = block.Hash;
    }
}
using System.Collections;
using BlockchainLib.Hash;

namespace BlockchainLib.BlockchainServices;

public class Blockchain : IBlockchain
{
    private readonly LinkedList<Block> _blocks;
    private readonly IHashFunction _hashFunction;
    
    public Blockchain(IHashFunction hashFunction)
    {
        _hashFunction = hashFunction;
        _blocks = new LinkedList<Block>();
    }

    public void AddBlock(Block block)
    {
        var tailHash = _blocks.LastOrDefault();
        if (block.ParentHash == tailHash?.Hash)
        {
            var expectedBlockHash = block.GetBlockHash(_hashFunction);
            
            if (expectedBlockHash == block.Hash)
                _blocks.AddLast(block);
            else
                throw new ApplicationException($"Block {block} has invalid hash. It should be {expectedBlockHash}.");
        }
        else
            throw new ApplicationException($"{block.ParentHash} is not following the current block {tailHash}");
    }
    public IEnumerator<Block> GetEnumerator() => _blocks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
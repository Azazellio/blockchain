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
            var expectedBlockHash = block.GetHash(_hashFunction);
            
            if (expectedBlockHash == block.Hash)
                _blocks.AddLast(block);
            else
                throw new ApplicationException($"Block {block} has invalid hash. It should be {expectedBlockHash}.");
        }
        else
            throw new ApplicationException($"{block.ParentHash} is not following the current block {tailHash}");
    }

    public IEnumerable<Block> GetBlocks(int amountOfBlocks)
    {
        if (_blocks == null)
            throw new ArgumentNullException(nameof(_blocks));

        if (amountOfBlocks <= 0)
            yield break;

        var node = _blocks.Last;
        for (int i = 0; i < amountOfBlocks && node != null; i++)
        {
            yield return node.Value;
            node = node.Previous;
        }
    }
    public IEnumerator<Block> GetEnumerator() => _blocks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
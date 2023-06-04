using System.Collections;
using System.Text.Json;
using BlockchainLib.BlockchainServices;
using HighLevelBlockchain.Indexes;
using HighLevelBlockchain.Rules.Abstractions;

namespace HighLevelBlockchain;

public class GenericBlockchain<T> : IGenericBlockchain<T>
{
    private readonly IBlockchainBuilder _blockchainBuilder;
    private readonly IBlockchain _blockchain;
    private readonly IRule<T>[] _rules;
    private readonly IIndexBuilder<T>[]? _indexes;

    public GenericBlockchain(
        IBlockchainBuilder blockchainBuilder,
        IBlockchain blockchain,
        IRule<T>[] rules,
        IIndexBuilder<T>[]? indexes = null)
    {
        _blockchainBuilder = blockchainBuilder;
        _blockchain = blockchain;
        _indexes = indexes;
        _rules = rules;
    }

    public GenericBlock<T> BuildBlock(T data)
    {
        var baseBlock = _blockchainBuilder.BuildBlock(JsonSerializer.Serialize(data));
        var genericBlock = 
            new GenericBlock<T>(
                baseBlock.Hash,
                baseBlock.ParentHash,
                baseBlock.Data,
                data);
        return genericBlock;
    }

    public void ProcessBlock(GenericBlock<T> genericBlock)
    {
        foreach (var rule in _rules)
        {
            rule.Execute(this, genericBlock);
        }

        var block = _blockchainBuilder.BuildBlock(genericBlock.Raw);
        _blockchain.AddBlock(block);
        _blockchainBuilder.ProcessBlock(block);

        if (_indexes != null)
            foreach (var index in _indexes)
            {
                index.IndexBlock(genericBlock.Data);
            }
    }

    public IEnumerator<GenericBlock<T>> GetEnumerator()
    {
        return _blockchain
            .Select(x => 
                new GenericBlock<T>(
                x.Hash,
                x.ParentHash,
                x.Data,
                JsonSerializer.Deserialize<T>(x.Data)!))
                .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<GenericBlock<T>> GetBlocks(int amountOfBlocks)
    {
        return _blockchain.GetBlocks(amountOfBlocks)
            .Select(x => new GenericBlock<T>(
                x.Hash,
                x.ParentHash,
                x.Data,
                JsonSerializer.Deserialize<T>(x.Data)!));
    }
}
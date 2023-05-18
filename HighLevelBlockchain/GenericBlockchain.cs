using System.Collections;
using System.Text.Json;
using BlockchainLib.BlockchainServices;
using BlockchainLib.BlockchainServices.BlockchainBuilder;
using HighLevelBlockchain.Rules;

namespace HighLevelBlockchain;

public class GenericBlockchain<T> : IEnumerable<GenericBlock<T>>, IGenericBlockchain<T>
{
    private readonly IBlockchainBuilder _blockchainBuilder;
    private readonly IBlockchain _blockchain;
    private readonly IRule<T>[] _rules;

    public GenericBlockchain(
        IBlockchainBuilder blockchainBuilder,
        IBlockchain blockchain,
        params IRule<T>[] rules)
    {
        _blockchainBuilder = blockchainBuilder;
        _blockchain = blockchain;
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
    
}
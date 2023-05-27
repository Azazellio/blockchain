using BlockChainApp.CustomIndexes.Abstractions;
using HighLevelBlockchain.Indexes;

namespace BlockChainApp.CustomIndexes;

public class PropertyIndex : IndexTyped<PropertyTransactionBlock, string>, IPropertyIndex
{
    public PropertyIndex() : base(x => x.TransactionPayload.Property) { }
}
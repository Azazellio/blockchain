using HighLevelBlockchain.Indexes;

namespace BlockChainApp.CustomIndexes.Abstractions;

public interface IPropertyIndex : IIndexTyped<PropertyTransactionBlock, string> { }
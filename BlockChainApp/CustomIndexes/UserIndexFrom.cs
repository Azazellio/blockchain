using BlockChainApp.CustomIndexes.Abstractions;
using HighLevelBlockchain.Indexes;

namespace BlockChainApp.CustomIndexes;

public class UserIndexFrom : IndexTyped<PropertyTransactionBlock, string>, IUserIndexFrom
{
    public UserIndexFrom() : base(x => x.TransactionPayload.From) { }
}
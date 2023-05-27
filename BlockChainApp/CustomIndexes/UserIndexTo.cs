using BlockChainApp.CustomIndexes.Abstractions;
using HighLevelBlockchain.Indexes;

namespace BlockChainApp.CustomIndexes;

public class UserIndexTo : IndexTyped<PropertyTransactionBlock, string>, IUserIndexTo
{
    public UserIndexTo() : base(x => x.TransactionPayload.To) { }
}
namespace HighLevelBlockchain.BlockContracts;

public interface ITransferBlock
{
    string From { get; }
    string To { get; }
}
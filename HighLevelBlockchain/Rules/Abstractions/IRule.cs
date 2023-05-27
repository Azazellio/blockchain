namespace HighLevelBlockchain.Rules.Abstractions;

public interface IRule<T>
{
    void Execute(IEnumerable<GenericBlock<T>> builtBlocks, GenericBlock<T> newData);
}
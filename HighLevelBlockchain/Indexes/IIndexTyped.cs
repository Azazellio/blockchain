namespace HighLevelBlockchain.Indexes;

public interface IIndexTyped<TBlock, TField>
{
    IEnumerable<TBlock> GetByKey(TField key);
}
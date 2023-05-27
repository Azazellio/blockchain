namespace HighLevelBlockchain.Indexes;

public interface IIndexBuilder<TBlock>
{
    void IndexBlock(TBlock newData);
}
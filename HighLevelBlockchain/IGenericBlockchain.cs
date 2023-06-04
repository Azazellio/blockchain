namespace HighLevelBlockchain;

public interface IGenericBlockchain<T> : IEnumerable<GenericBlock<T>>
{
    public GenericBlock<T> BuildBlock(T data);

    public void ProcessBlock(GenericBlock<T> genericBlock);

    public IEnumerable<GenericBlock<T>> GetBlocks(int amountOfBlocks);
}
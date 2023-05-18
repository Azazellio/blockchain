namespace HighLevelBlockchain;

public interface IGenericBlockchain<T>
{
    public GenericBlock<T> BuildBlock(T data);

    public void ProcessBlock(GenericBlock<T> genericBlock);
}
namespace BlockchainLib.BlockchainServices.BlockchainBuilder;

public interface IBlockchainBuilder
{
    public Block BuildBlock(string data);

    public void ProcessBlock(Block block);

}
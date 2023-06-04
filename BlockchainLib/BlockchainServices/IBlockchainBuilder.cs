namespace BlockchainLib.BlockchainServices;

public interface IBlockchainBuilder
{
    public Block BuildBlock(string data);

    public void ProcessBlock(Block block);

}
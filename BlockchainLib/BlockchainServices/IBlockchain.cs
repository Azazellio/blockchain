namespace BlockchainLib.BlockchainServices;

public interface IBlockchain : IEnumerable<Block>
{
    public void AddBlock(Block block);
    public IEnumerable<Block> GetBlocks(int amountOfBlocks);
}
using HighLevelBlockchain.BlockContracts;
using HighLevelBlockchain.Rules.Abstractions;

namespace HighLevelBlockchain.Rules;

public interface IPropertyOwnershipBlock : ITransferBlock
{
    string Property { get; }
}

public class PropertyOwnershipRule<TBlock> : IRule<TBlock> where TBlock : IPropertyOwnershipBlock
{
    public void Execute(IEnumerable<GenericBlock<TBlock>> builtBlocks, GenericBlock<TBlock> newData)
    {
        var block = newData.Data;
        if (block.From == block.To)
        {
            // Case of registration of new property
            // Verify that nobody else has registered this Property (uniqueness)
            if (builtBlocks.Any(b => b.Data.Property == block.Property))
                throw new ApplicationException(
                    "You are trying to register property that is already registered.");
        }
        else
        {
            // Verify that the person who is transferring this Property is it's owner
            foreach (var b in builtBlocks.Reverse())
            {
                if (b.Data.Property == block.Property)
                {
                    if (block.From == b.Data.To)
                        return;
                    throw new ApplicationException(
                        "You are trying to transfer property that you are not owning.");
                }
            }
            // In case this Property hasn't been registered in blockchain
            throw new ApplicationException("You are trying to transfer property that has not been yet registered.");
        }
    }
}
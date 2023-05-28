using HighLevelBlockchain;
using HighLevelBlockchain.Rules;

namespace BlockchainAppTests.Rules;

public class TestPropertyOwnershipBlock : IPropertyOwnershipBlock
{
    public string From { get; set; }
    public string To { get; set; }
    public string Property { get; set; }
}

[TestFixture]
public class PropertyOwnershipRuleTests
{
    private const string PropertyName = "Art1";
    private const string Owner1 = "Owner1";
    private const string Owner2 = "Owner2";

    private GenericBlock<TestPropertyOwnershipBlock> CreateBlock(string from, string to, string property)
    {
        return new GenericBlock<TestPropertyOwnershipBlock>(null, null, null, new TestPropertyOwnershipBlock
        {
            From = from,
            To = to,
            Property = property
        });
    }

    [Test]
    public void Execute_WhenRegisteringAlreadyRegisteredProperty_ThrowsException()
    {
        var rule = new PropertyOwnershipRule<TestPropertyOwnershipBlock>();
        var existingBlocks = new List<GenericBlock<TestPropertyOwnershipBlock>>
        {
            CreateBlock(Owner1, Owner1, PropertyName)
        };
        var newBlock = CreateBlock(Owner2, Owner2, PropertyName);

        Assert.Throws<ApplicationException>(() => rule.Execute(existingBlocks, newBlock));
    }

    [Test]
    public void Execute_WhenTransferringNotOwnedProperty_ThrowsException()
    {
        var rule = new PropertyOwnershipRule<TestPropertyOwnershipBlock>();
        var existingBlocks = new List<GenericBlock<TestPropertyOwnershipBlock>>
        {
            CreateBlock(Owner1, Owner1, PropertyName)
        };
        var newBlock = CreateBlock(Owner2, Owner1, PropertyName);

        Assert.Throws<ApplicationException>(() => rule.Execute(existingBlocks, newBlock));
    }

    [Test]
    public void Execute_WhenTransferringNotRegisteredProperty_ThrowsException()
    {
        var rule = new PropertyOwnershipRule<TestPropertyOwnershipBlock>();
        var existingBlocks = new List<GenericBlock<TestPropertyOwnershipBlock>>();
        var newBlock = CreateBlock(Owner1, Owner2, PropertyName);

        Assert.Throws<ApplicationException>(() => rule.Execute(existingBlocks, newBlock));
    }

    [Test]
    public void Execute_WhenTransferringOwnedProperty_DoesNotThrowException()
    {
        var rule = new PropertyOwnershipRule<TestPropertyOwnershipBlock>();
        var existingBlocks = new List<GenericBlock<TestPropertyOwnershipBlock>>
        {
            CreateBlock(Owner1, Owner1, PropertyName)
        };
        var newBlock = CreateBlock(Owner1, Owner2, PropertyName);

        Assert.DoesNotThrow(() => rule.Execute(existingBlocks, newBlock));
    }
}

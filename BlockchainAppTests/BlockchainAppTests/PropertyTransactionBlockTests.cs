using BlockChainApp;
using HighLevelBlockchain.BlockContracts;
using HighLevelBlockchain.Rules;

namespace BlockchainAppTests.BlockchainAppTests;

[TestFixture]
public class PropertyTransactionBlockTests
{
    private const string TestFrom = "testFrom";
    private const string TestTo = "testTo";
    private const string TestProperty = "testProperty";
    private DateTime TestTime = DateTime.Now;
    private const string TestSign = "testSign";

    private PropertyTransactionBlock _block;

    [SetUp]
    public void SetUp()
    {
        var transaction = new PropertyTransaction(TestFrom, TestTo, TestProperty, TestTime);
        _block = new PropertyTransactionBlock(transaction, TestSign);
    }

    [Test]
    public void PublicKey_Should_Return_Correct_Value()
    {
        Assert.AreEqual(TestFrom, ((ISignedBlock<PropertyTransaction>)_block).PublicKey);
    }

    [Test]
    public void Data_Should_Return_Correct_Value()
    {
        var expectedData = new PropertyTransaction(TestFrom, TestTo, TestProperty, TestTime);
        Assert.AreEqual(expectedData, ((ISignedBlock<PropertyTransaction>)_block).Data);
    }

    [Test]
    public void From_Should_Return_Correct_Value()
    {
        Assert.AreEqual(TestFrom, ((ITransferBlock)_block).From);
    }

    [Test]
    public void To_Should_Return_Correct_Value()
    {
        Assert.AreEqual(TestTo, ((ITransferBlock)_block).To);
    }

    [Test]
    public void Property_Should_Return_Correct_Value()
    {
        Assert.AreEqual(TestProperty, ((IPropertyOwnershipBlock)_block).Property);
    }
}
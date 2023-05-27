using System.Text.Json;
using BlockchainLib.Cryptography;
using HighLevelBlockchain;
using HighLevelBlockchain.BlockContracts;
using HighLevelBlockchain.Rules;

namespace BlockChainApp;

public record PropertyTransaction(string From, string To, string Property, DateTime Time);
public record PropertyTransactionBlock(PropertyTransaction TransactionPayload, string Sign) 
    : ISignedBlock<PropertyTransaction>
        , IPropertyOwnershipBlock
{
    // Fields to support sign rule
    string ISignedBlock<PropertyTransaction>.PublicKey => TransactionPayload.From;
    PropertyTransaction ISignedBlock<PropertyTransaction>.Data => TransactionPayload;
    
    // Fields to support transferring rule
    string ITransferBlock.From => TransactionPayload.From;
    string ITransferBlock.To => TransactionPayload.To;
    
    // Fields to support Property uniqueness rule
    string IPropertyOwnershipBlock.Property => TransactionPayload.Property;
}

public class SimpleApp
{
    private readonly IGenericBlockchain<PropertyTransactionBlock> _blockchain;
    private readonly IEncryptorService _encryptorService;

    public SimpleApp(IEncryptorService encryptorService, IGenericBlockchain<PropertyTransactionBlock> blockchain)
    {
        _encryptorService = encryptorService;
        _blockchain = blockchain;
    }
    
    public Keys GenerateKeys() => _encryptorService.GenerateKeyPair();

    private void AcceptTransaction(PropertyTransactionBlock transactionBlock)
    {
        var block = _blockchain.BuildBlock(transactionBlock);
        _blockchain.ProcessBlock(block);
    }
    
    public void PerformTransaction(Keys fromKeys, string to, string property)
    {
        var transaction = new PropertyTransaction(fromKeys.Public, to, property, DateTime.Now);
        var transactionString = JsonSerializer.Serialize(transaction);
        var sign = _encryptorService.SignData(transactionString, fromKeys.Private);
        var transactionBlock = new PropertyTransactionBlock(transaction, sign);

        AcceptTransaction(transactionBlock);
    }
}
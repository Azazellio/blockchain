using System.Text.Json;
using BlockchainLib.BlockchainServices;
using BlockchainLib.BlockchainServices.BlockchainBuilder;
using BlockchainLib.Cryptography;
using BlockchainLib.Hash;
using HighLevelBlockchain;
using HighLevelBlockchain.BlockContracts;
using HighLevelBlockchain.Rules;

namespace ConsoleBlockChainApp;

public record PropertyTransaction(string From, string To, string Property);
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
    
    public KeyPair GenerateKeys() => _encryptorService.GenerateKeys();

    private void AcceptTransaction(PropertyTransactionBlock transactionBlock)
    {
        var block = _blockchain.BuildBlock(transactionBlock);
        _blockchain.ProcessBlock(block);
    }
    
    public void PerformTransaction(KeyPair fromKeys, string to, string property)
    {
        var transaction = new PropertyTransaction(fromKeys.PublicKey, to, property);
        var transactionString = JsonSerializer.Serialize(transaction);
        var sign = _encryptorService.Sign(transactionString, fromKeys.PrivateKey);
        var transactionBlock = new PropertyTransactionBlock(transaction, sign);

        AcceptTransaction(transactionBlock);
    }
}
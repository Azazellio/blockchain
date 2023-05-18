using BlockchainLib.BlockchainServices;
using BlockchainLib.BlockchainServices.BlockchainBuilder;
using BlockchainLib.Cryptography;
using BlockchainLib.Hash;
using ConsoleBlockChainApp;
using HighLevelBlockchain;
using HighLevelBlockchain.Rules;

// var sampleApp = new SampleSign();
// sampleApp.DoSample();

var hashfunc = new CRC32Hash();
var blockchainBase = new Blockchain(hashfunc);
var blockchainBuilderBase = new BlockchainBuilder(hashfunc, null);

var encryptorService = new RSAEncryptor();
        
var genericBlockchain = new GenericBlockchain<PropertyTransactionBlock>(
    blockchainBuilderBase,
    blockchainBase,
    new SignRule<PropertyTransactionBlock, PropertyTransaction>(encryptorService),
    new PropertyOwnershipRule<PropertyTransactionBlock>());

var app = new SimpleApp(encryptorService, genericBlockchain);

var user1 = app.GenerateKeys();
var user2 = app.GenerateKeys();

app.PerformTransaction(user1, user1.PublicKey, "some prop");
app.PerformTransaction(user1, user2.PublicKey, "some prop");
app.PerformTransaction(user1, user2.PublicKey, "some prop1");


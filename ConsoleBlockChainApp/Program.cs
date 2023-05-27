using BlockChainApp;
using BlockchainLib.BlockchainServices;
using BlockchainLib.BlockchainServices.BlockchainBuilder;
using BlockchainLib.Cryptography;
using BlockchainLib.Hash;
using HighLevelBlockchain;
using HighLevelBlockchain.Indexes;
using HighLevelBlockchain.Rules;
using HighLevelBlockchain.Rules.Abstractions;

// var sampleApp = new SampleSign();
// sampleApp.DoSample();

var hashfunc = new CRC32Hash();
var blockchainBase = new Blockchain(hashfunc);
var blockchainBuilderBase = new BlockchainBuilder(hashfunc, null);

var encryptorService = new RSAEncryptor();

var rules = new List<IRule<PropertyTransactionBlock>>
{
    new SignRule<PropertyTransactionBlock, PropertyTransaction>(encryptorService),
    new PropertyOwnershipRule<PropertyTransactionBlock>()
};

var propertyIndex = new IndexTyped<PropertyTransactionBlock, string>(x => x.TransactionPayload.Property);
var userIndexTo = new IndexTyped<PropertyTransactionBlock, string>(x => x.TransactionPayload.To);
var userIndexFrom = new IndexTyped<PropertyTransactionBlock, string>(x => x.TransactionPayload.From);

var indexes = new List<IIndexBuilder<PropertyTransactionBlock>>
{
    propertyIndex,
    userIndexFrom,
    userIndexTo
};
        
var genericBlockchain = new GenericBlockchain<PropertyTransactionBlock>(
    blockchainBuilderBase,
    blockchainBase,
    rules.ToArray(),
    indexes.ToArray()
    );

var app = new SimpleApp(encryptorService, genericBlockchain);

var user1 = app.GenerateKeys();
var user2 = app.GenerateKeys();

app.PerformTransaction(user1, user1.Public, "some prop");
app.PerformTransaction(user2, user2.Public, "some prop1");
app.PerformTransaction(user1, user2.Public, "some prop");
var history = propertyIndex.GetByKey("some prop");
var historyFrom = userIndexFrom.GetByKey(user1.Public);
var historyTo = userIndexTo.GetByKey(user2.Public);

app.PerformTransaction(user1, user2.Public, "some prop1");


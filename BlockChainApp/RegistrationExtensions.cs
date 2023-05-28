using BlockChainApp.CustomIndexes;
using BlockChainApp.CustomIndexes.Abstractions;
using BlockchainLib.BlockchainServices;
using BlockchainLib.BlockchainServices.BlockchainBuilder;
using BlockchainLib.Cryptography;
using BlockchainLib.Hash;
using HighLevelBlockchain;
using HighLevelBlockchain.Indexes;
using HighLevelBlockchain.Rules;
using HighLevelBlockchain.Rules.Abstractions;

namespace BlockChainApp;

public static class RegistrationExtensions
{
    public static void RegisterDependencies(this IServiceCollection services)
    { 
        services.AddTransient<IHashFunction, CRC32Hash>();
        services.AddSingleton<IBlockchain, Blockchain>();
        services.AddSingleton<IBlockchainBuilder, BlockchainBuilder>(_ 
            => new BlockchainBuilder(_.GetRequiredService<IHashFunction>(), null));
        services.AddTransient<IEncryptorService, RSAEncryptor>();
        
        services.RegisterRules();
        services.RegisterIndexes();
        
        services
            .AddTransient<IGenericBlockchain<PropertyTransactionBlock>, GenericBlockchain<PropertyTransactionBlock>>();

        services.AddTransient<ISimpleApp, SimpleApp>();
    }

    private static void RegisterRules(this IServiceCollection services)
    {
        services.AddTransient<IRule<PropertyTransactionBlock>, PropertyOwnershipRule<PropertyTransactionBlock>>();
        services.AddTransient<IRule<PropertyTransactionBlock>, SignRule<PropertyTransactionBlock, PropertyTransaction>>();
        
        services.AddTransient<IRule<PropertyTransactionBlock>[]>(provider =>
        {
            var rules = provider.GetServices<IRule<PropertyTransactionBlock>>();
            return rules.ToArray();
        });
    }

    private static void RegisterIndexes(this IServiceCollection services)
    {
        services.AddSingleton<IIndexBuilder<PropertyTransactionBlock>>(_ => (IIndexBuilder<PropertyTransactionBlock>)_.GetRequiredService<IPropertyIndex>());
        services.AddSingleton<IIndexBuilder<PropertyTransactionBlock>>(_ => (IIndexBuilder<PropertyTransactionBlock>)_.GetRequiredService<IUserIndexFrom>());
        services.AddSingleton<IIndexBuilder<PropertyTransactionBlock>>(_ => (IIndexBuilder<PropertyTransactionBlock>)_.GetRequiredService<IUserIndexTo>());

        services.AddSingleton<IPropertyIndex, PropertyIndex>();
        services.AddSingleton<IUserIndexFrom, UserIndexFrom>();
        services.AddSingleton<IUserIndexTo, UserIndexTo>();

        services.AddTransient<IIndexBuilder<PropertyTransactionBlock>[]>(provider =>
        {
            var indexes = provider.GetServices<IIndexBuilder<PropertyTransactionBlock>>();
            return indexes.ToArray();
        });
    }
}
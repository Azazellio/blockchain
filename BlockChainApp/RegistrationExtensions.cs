using BlockchainLib.BlockchainServices;
using BlockchainLib.BlockchainServices.BlockchainBuilder;
using BlockchainLib.Cryptography;
using BlockchainLib.Hash;
using ConsoleBlockChainApp;
using HighLevelBlockchain;
using HighLevelBlockchain.Rules;

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
        
        // services
        //     .AddTransient<IGenericBlockchain<PropertyTransactionBlock>>(_ 
        //         =>
        //     {
        //         var builder = 
        //         return new GenericBlockchain<PropertyTransactionBlock>(IBlockchainBuilder builder,);
        //     });
        services
            .AddTransient<IGenericBlockchain<PropertyTransactionBlock>, GenericBlockchain<PropertyTransactionBlock>>();

        services.AddTransient<SimpleApp>();
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
}
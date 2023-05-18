// using System.Text.Json;
// using BlockchainLib;
// using BlockchainLib.BlockchainServices;
// using BlockchainLib.Hash;
//
// namespace HighLevelBlockchain;
//
// public class GenericBlockchainBuilder<T> : IGenericBlockchainBuilder<T>
// {
//     private readonly IBlockchainBuilder _blockchainBuilder;
//     
//     public GenericBlockchainBuilder(
//         IBlockchainBuilder blockchainBuilder,
//         IHashFunction hashFunction,
//         string? lastBlockHash)
//     {
//         _blockchainBuilder = blockchainBuilder;
//     }
//
//
//
//     public void ProcessBlock(GenericBlock<T> block)
//     {
//         _blockchainBuilder.ProcessBlock(block);
//     }
// }
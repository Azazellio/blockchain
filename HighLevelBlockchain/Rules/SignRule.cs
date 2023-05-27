using System.Text.Json;
using BlockchainLib.Cryptography;
using HighLevelBlockchain.Rules.Abstractions;

namespace HighLevelBlockchain.Rules;

public interface ISignedBlock<T>
{
    public string Sign { get; }
    public string PublicKey { get; }
    public T Data { get; }
}

public class SignRule<TBlock, TSignedBlockPayload> : IRule<TBlock> where TBlock : ISignedBlock<TSignedBlockPayload>
{
    private readonly IEncryptorService _encryptor;
    
    public SignRule(IEncryptorService encryptor)
    {
        _encryptor = encryptor;
    }
    public void Execute(IEnumerable<GenericBlock<TBlock>> _, GenericBlock<TBlock> newBlock)
    {
        var signed = newBlock.Data;
        var dataThatShouldBeSigned = JsonSerializer.Serialize(newBlock.Data.Data);
        if (!_encryptor.VerifySignature(signed.PublicKey, dataThatShouldBeSigned, signed.Sign))
            throw new ApplicationException("Block sign is incorrect.");
    }
}
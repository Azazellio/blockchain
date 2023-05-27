using BlockchainLib.Cryptography;

namespace ConsoleBlockChainApp.Samples;

public class SampleSign
{
    public void DoSample()
    {
        var rsaEncryptor = new RSAEncryptor();
        var keyPair = rsaEncryptor.GenerateKeyPair();
        Console.WriteLine(keyPair);

        var someData = "Hello World";
        
        var signedBlock = rsaEncryptor.SignData(someData, keyPair.Private);
        if (rsaEncryptor.VerifySignature(keyPair.Public, someData, signedBlock))
            Console.WriteLine("Signed correctly");
        else
            Console.Error.WriteLine("Signed incorrectly");

        if (rsaEncryptor.VerifySignature(keyPair.Public, someData, signedBlock.Replace('A', 'B')))
            Console.WriteLine("Signed correctly");
        else
            Console.Error.WriteLine("Signed incorrectly");

        var anotherPair = rsaEncryptor.GenerateKeyPair();
        if (rsaEncryptor.VerifySignature(anotherPair.Public, someData, signedBlock))
            Console.WriteLine("Signed correctly");
        else
            Console.Error.WriteLine("Signed incorrectly");
    }
}
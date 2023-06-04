namespace HighLevelBlockchain;

public record struct GenericBlock<T>(string Hash, string ParentHash, string Raw, T Data);
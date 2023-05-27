namespace HighLevelBlockchain.Indexes;

public class IndexTyped<TBlock, TField> : IIndexTyped<TBlock, TField>, IIndexBuilder<TBlock> where TField : notnull
{
    private readonly Dictionary<TField, List<TBlock>> _hashMap;
    private readonly Func<TBlock, TField> _keyGetter;

    public IndexTyped(Func<TBlock, TField> keyGetter)
    {
        _hashMap = new Dictionary<TField, List<TBlock>>();
        _keyGetter = keyGetter;
    }
    public void IndexBlock(TBlock newData)
    {
        var key = _keyGetter(newData);
        if(_hashMap.TryGetValue(key, out var value))
            value.Add(newData);
        else
        {
            _hashMap.Add(key, new List<TBlock>{ newData });
        }
    }

    public IEnumerable<TBlock> GetByKey(TField key)
    {
        if (_hashMap.TryGetValue(key, out var value))
            return value;
        else
        {
            return Enumerable.Empty<TBlock>();
        }
    }
}
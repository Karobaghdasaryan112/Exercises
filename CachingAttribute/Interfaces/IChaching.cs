

namespace CachingAttribute.Interfaces
{
    public interface IChaching
    {
        object GetOrSet(string ChachKey,object res);
        bool TryGetValue(string ChachKey, out object Value);
    }
}

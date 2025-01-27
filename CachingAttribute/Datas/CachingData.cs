

namespace CachingAttribute.Datas
{
    public class CachingData
    {
        private Dictionary<string, object> _data;
        public CachingData()
        {
            _data = new Dictionary<string, object>();
        }
        public void Set(string key, object value)
        {
            _data[key] = value;
        }
        public bool Get(string key,out object result)
        {
            if (_data.ContainsKey(key))
            {
                 result = _data[key];
                return true;
            }
            result = default; 
            return false;
        }
    }
}

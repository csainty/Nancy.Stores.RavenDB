namespace Nancy.Stores.RavenDB.Models
{
    using Newtonsoft.Json;

    public class CacheItem
    {
        public string Id { get; set; }

        public string Value { get; set; }

        public T GetValue<T>() {
            return JsonConvert.DeserializeObject<T>(Value);
        }

        public static CacheItem Create(string id, object value)
        {
            return new CacheItem { Id = id, Value = JsonConvert.SerializeObject(value) };
        }
    }
}

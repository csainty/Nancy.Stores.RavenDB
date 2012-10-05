namespace Nancy.Stores.RavenDB
{
    using Cache;
    using Nancy.Stores.RavenDB.Models;
    using Raven.Client;

    public class RavenDbCacheStore : ICacheStore
    {
        private readonly IDocumentStore documentStore;

        public RavenDbCacheStore(IDocumentStore documentStore)
        {
            Guard.NotNull(() => documentStore, documentStore);

            this.documentStore = documentStore;
        }

        public void Remove(string id)
        {
            Guard.NotNullOrEmpty(() => id, id);

            using (var session = documentStore.OpenSession()) {
                var item = session.Load<Models.CacheItem>(id);

                if (item != null)
                {
                    session.Delete(item);
                    session.SaveChanges();
                }
            }
        }

        public void Store(string id, object obj)
        {
            Guard.NotNullOrEmpty(() => id, id);

            var item = CacheItem.Create(id, obj);

            using (var session = documentStore.OpenSession()) {
                session.Store(item);
                session.SaveChanges();
            }
        }

        public bool TryLoad<TItem>(string id, out TItem obj)
        {
            Guard.NotNullOrEmpty(() => id, id);

            using (var session = documentStore.OpenSession()) {
                var item = session.Load<CacheItem>(id);

                if (item == null) {
                    obj = default(TItem);
                    return false;
                }

                obj = item.GetValue<TItem>();
                return true;
            }
        }
    }
}

namespace Nancy.Stores.RavenDB
{
    using System;
    using Cache;
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
        }

        public bool TryLoad<T>(string id, out T obj)
        {
            Guard.NotNullOrEmpty(() => id, id);

            throw new NotImplementedException();
        }
    }
}

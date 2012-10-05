namespace Nancy.Stores.RavenDB
{
    using System;
    using Nancy.Cache;
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
            throw new NotImplementedException();
        }

        public void Store(string id, object obj)
        {
            throw new NotImplementedException();
        }

        public bool TryLoad<T>(string id, out T obj)
        {
            throw new NotImplementedException();
        }
    }
}

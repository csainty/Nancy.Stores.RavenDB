namespace Nancy.Stores.RavenDB
{
    using System;
    using System.Collections.Generic;
    using Cryptography;
    using Raven.Client;
    using Session;

    public class RavenDbSessionStore : AbstractIdBasedSessionStore
    {
        private readonly IDocumentStore documentStore;

        public RavenDbSessionStore(CryptographyConfiguration cryptographyConfiguration, IDocumentStore documentStore)
            : base(cryptographyConfiguration)
        {
            Guard.NotNull(() => documentStore, documentStore);

            this.documentStore = documentStore;
        }

        protected override void Save(string id, IDictionary<string, object> items)
        {
            Guard.NotNullOrEmpty(() => id, id);
            Guard.NotNull(() => items, items);

            var session = new Models.Session
            {
                Id = Guid.Parse(id),
                Items = items
            };

            using (var work = documentStore.OpenSession())
            {
                work.Store(session);
                work.SaveChanges();
            }
        }

        protected override bool TryLoad(string id, out IDictionary<string, object> items)
        {
            using (var work = documentStore.OpenSession())
            {
                var session = work.Load<Models.Session>(id);

                if (null == session)
                {
                    items = new Dictionary<string, object>();
                    return false;
                }

                items = session.Items;
                return true;
            }
        }
    }
}

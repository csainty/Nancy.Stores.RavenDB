﻿namespace Nancy.Stores.RavenDB
{
    using System;
    using System.Collections.Generic;
    using Cryptography;
    using Raven.Client;
    using Session;

    public class RavenDbSessionStore : AbstractIdBasedSessionStore
    {
        private readonly IDocumentStore documentStore;

        public RavenDbSessionStore(CryptographyConfiguration cryptographyConfiguration, IDocumentStore documentStore) : base(cryptographyConfiguration)
        {
            Guard.NotNull(() => documentStore, documentStore);

            this.documentStore = documentStore;
        }

        protected override void Save(string id, IDictionary<string, object> items)
        {
            throw new NotImplementedException();
        }

        protected override bool TryLoad(string id, out IDictionary<string, object> items)
        {
            throw new NotImplementedException();
        }
    }
}

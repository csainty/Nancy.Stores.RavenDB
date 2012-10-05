namespace Nancy.Stores.RavenDB.Tests.Unit
{
    using System;
    using FakeItEasy;
    using Raven.Client;
    using Should;
    using Xunit;

    public class RavenDbCacheStoreFixture
    {
        private readonly IDocumentStore documentStore;
        private readonly IDocumentSession documentSession;
        private readonly RavenDbCacheStore cacheStore;

        public RavenDbCacheStoreFixture()
        {
            this.documentStore = A.Fake<IDocumentStore>();
            this.documentSession = A.Fake<IDocumentSession>();
            A.CallTo(() => documentStore.OpenSession()).Returns(this.documentSession);

            this.cacheStore = new RavenDbCacheStore(this.documentStore);
        }

        [Fact]
        public void Constructor_throws_argument_null_exception_when_no_document_store_passed() {
            // When
            var result = Record.Exception(() => new RavenDbCacheStore(null));

            // Then
            result.ShouldBeType<ArgumentNullException>();
        }
    }
}

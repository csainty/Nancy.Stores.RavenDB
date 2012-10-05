namespace Nancy.Stores.RavenDB.Tests.Unit
{
    using System;
    using FakeItEasy;
    using Nancy.Stores.RavenDB.Models;
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
        public void Constructor_throws_argument_null_exception_when_no_document_store_passed()
        {
            // When
            var result = Record.Exception(() => new RavenDbCacheStore(null));

            // Then
            result.ShouldBeType<ArgumentNullException>();
        }

        [Fact]
        public void Remove_should_throw_argumentnullexception_when_id_is_null()
        {
            // When
            var result = Record.Exception(() => cacheStore.Remove(null));

            // Then
            result.ShouldBeType<ArgumentNullException>();
        }

        [Fact]
        public void Remove_should_throw_argumentexception_when_id_is_empty()
        {
            // When
            var result = Record.Exception(() => cacheStore.Remove(""));

            // Then
            result.ShouldBeType<ArgumentException>();
        }

        [Fact]
        public void Store_should_throw_argumentnullexception_when_id_is_null()
        {
            // When
            var result = Record.Exception(() => cacheStore.Store(null, "value"));

            // Then
            result.ShouldBeType<ArgumentNullException>();
        }

        [Fact]
        public void Store_should_throw_argumentexception_when_id_is_empty()
        {
            // When
            var result = Record.Exception(() => cacheStore.Store("", "value"));

            // Then
            result.ShouldBeType<ArgumentException>();
        }

        [Fact]
        public void TryLoad_should_throw_argumentnullexception_when_id_is_null()
        {
            // When
            string _;
            var result = Record.Exception(() => cacheStore.TryLoad(null, out _));

            // Then
            result.ShouldBeType<ArgumentNullException>();
        }

        [Fact]
        public void TryLoad_should_throw_argumentexception_when_id_is_empty()
        {
            // When
            string _;
            var result = Record.Exception(() => cacheStore.TryLoad("", out _));

            // Then
            result.ShouldBeType<ArgumentException>();
        }

        [Fact]
        public void Remove_should_not_change_the_database_if_the_cache_item_doesnt_exist()
        {
            // Given
            A.CallTo(() => documentSession.Load<CacheItem>(A<string>.Ignored)).Returns(null);

            // When
            cacheStore.Remove("id");

            // Then
            A.CallTo(() => documentSession.SaveChanges()).MustNotHaveHappened();
        }

        [Fact]
        public void Remove_should_remove_the_item_from_ravendb()
        {
            // Given
            A.CallTo(() => documentSession.Load<CacheItem>("id")).Returns(new CacheItem { Id = "id", Value = "value" });

            // When
            cacheStore.Remove("id");

            // Then
            A.CallTo(() => documentSession.Delete(A<CacheItem>.That.Matches(i => i.Id == "id"))).MustHaveHappened();
        }

        [Fact]
        public void Remove_should_call_savechanges_on_ravendb()
        {
            // Given
            A.CallTo(() => documentSession.Load<CacheItem>("id")).Returns(new CacheItem { Id = "id", Value = "value" });

            // When
            cacheStore.Remove("id");

            // Then
            A.CallTo(() => documentSession.SaveChanges()).MustHaveHappened();
        }

        [Fact]
        public void Store_should_store_the_document_in_ravendb()
        {
            // Given
            var id = "123";
            var value = 1;

            // When
            cacheStore.Store(id, value);

            // Then
            A.CallTo(() => documentSession.Store(A<CacheItem>.That.Matches(i => i.Id == id && (int)i.Value == value))).MustHaveHappened();
        }

        [Fact]
        public void Store_should_call_save_changes_on_ravendb()
        {
            // Given

            // When
            cacheStore.Store("id", "value");

            // Then
            A.CallTo(() => documentSession.SaveChanges()).MustHaveHappened();
        }

        [Fact]
        public void TryLoad_should_return_false_when_item_not_found()
        {
            // Given
            string _;
            A.CallTo(() => documentSession.Load<CacheItem>("id")).Returns(null);

            // When
            bool result = cacheStore.TryLoad("id", out _);

            // Then
            result.ShouldBeFalse();
        }

        [Fact]
        public void TryLoad_should_return_true_when_item_is_found()
        {
            // Given
            int _;
            var item = new CacheItem { Id = "id", Value = 1 };
            A.CallTo(() => documentSession.Load<CacheItem>("id")).Returns(item);

            // When
            bool result = cacheStore.TryLoad("id", out _);

            // Then
            result.ShouldBeTrue();
        }

        [Fact]
        public void TryLoad_should_set_the_loaded_item_in_the_out_parameter()
        {
            // Given
            string item;
            var cacheItem = new CacheItem { Id = "id", Value = "value" };
            A.CallTo(() => documentSession.Load<CacheItem>("id")).Returns(cacheItem);

            // When
            cacheStore.TryLoad("id", out item);

            // Then
            item.ShouldEqual("value");
        }
    }
}

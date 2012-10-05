namespace Nancy.Stores.RavenDB.Tests.Integration
{
    using System;
    using Raven.Client;
    using Raven.Client.Embedded;
    using Should;
    using Xunit;

    public class RavenDbCacheStoreFixture : IDisposable
    {
        private readonly IDocumentStore documentStore;
        private readonly RavenDbCacheStore cacheStore;

        public RavenDbCacheStoreFixture()
        {
            documentStore = new EmbeddableDocumentStore { RunInMemory = true };
            documentStore.Initialize();
            cacheStore = new RavenDbCacheStore(documentStore);
        }

        [Fact]
        public void Can_store_then_retrieve_a_string_from_the_cache()
        {
            // Given
            string value;
            cacheStore.Store("123", "Hello");

            // when
            var result = cacheStore.TryLoad("123", out value);

            // Then
            result.ShouldBeTrue();
            value.ShouldEqual("Hello");
        }

        [Fact]
        public void Can_store_then_retrieve_a_number_from_the_cache()
        {
            // Given
            int value;
            cacheStore.Store("321", 321);

            // when
            var result = cacheStore.TryLoad("321", out value);

            // Then
            result.ShouldBeTrue();
            value.ShouldEqual(321);
        }

        [Fact]
        public void Can_store_then_retrieve_a_datetime_from_the_cache()
        {
            // Given
            DateTime value;
            cacheStore.Store("dt", new DateTime(2012, 01, 01));

            // when
            var result = cacheStore.TryLoad("dt", out value);

            // Then
            result.ShouldBeTrue();
            value.ShouldEqual(new DateTime(2012, 01, 01));
        }

        [Fact]
        public void Can_store_then_retrieve_a_complex_object_from_the_cache()
        {
            // Given
            Person value;
            cacheStore.Store("dt", new Person { Name = "Chris", Age = 30 });

            // when
            var result = cacheStore.TryLoad("dt", out value);

            // Then
            result.ShouldBeTrue();
            value.Age.ShouldEqual(30);
            value.Name.ShouldEqual("Chris");
        }

        public void Dispose()
        {
            using (documentStore) { }
        }
    }

    public class Person {

        public string Name { get; set; }

        public int Age { get; set; }
    }
}

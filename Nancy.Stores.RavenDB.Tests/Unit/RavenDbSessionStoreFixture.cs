namespace Nancy.Stores.RavenDB.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using Nancy.Cryptography;
    using Nancy.Stores.RavenDB.Tests.Fakes;
    using Raven.Client;
    using Should;
    using Xunit;

    public class RavenDbSessionStoreFixture
    {
        private readonly RavenDbSessionStore sessionStore;
        private readonly IDocumentStore documentStore;
        private readonly IDocumentSession documentSession;
        private readonly CryptographyConfiguration cryptographyConfiguration = FakeCryptographyConfiguration.Configuration;

        public RavenDbSessionStoreFixture()
        {
            this.documentStore = A.Fake<IDocumentStore>();
            this.documentSession = A.Fake<IDocumentSession>();
            A.CallTo(() => this.documentStore.OpenSession()).Returns(this.documentSession);

            sessionStore = new RavenDbSessionStore(this.cryptographyConfiguration, this.documentStore);
        }

        [Fact]
        public void Should_throw_argumentnullexception_when_no_documentstore_given()
        {
            // Given
            var result = Record.Exception(() =>
            {
                var store = new RavenDbSessionStore(A.Fake<CryptographyConfiguration>(), null);
            });

            // Then
            result.ShouldBeType<ArgumentNullException>();
        }

        [Fact]
        public void TryLoad_should_return_false_when_session_not_found()
        {
            // Given
            IDictionary<string, object> items;
            A.CallTo(() => documentSession.Load<Models.Session>(A<string>.Ignored)).Returns(null);

            // When
            var result = sessionStore.TryLoadSession(BuildRequestWithSession(Guid.NewGuid()), out items);

            // Then
            result.ShouldBeFalse();
            A.CallTo(() => documentSession.Load<Models.Session>(A<string>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public void TryLoad_should_not_leave_items_null_when_session_not_found()
        {
            // Given
            IDictionary<string, object> items;
            A.CallTo(() => documentSession.Load<Models.Session>(A<string>.Ignored)).Returns(null);

            // When
            sessionStore.TryLoadSession(BuildRequestWithSession(Guid.NewGuid()), out items);

            // Then
            items.ShouldNotBeNull();
            items.Count.ShouldEqual(0);
            A.CallTo(() => documentSession.Load<Models.Session>(A<string>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public void TryLoad_should_return_true_when_session_is_loaded()
        {
            // Given
            IDictionary<string, object> items;
            IDictionary<string, object> sessionItems = new Dictionary<string, object> { { "test", "value" } };
            var session = new Models.Session { Id = Guid.NewGuid(), Items = sessionItems };
            A.CallTo(() => documentSession.Load<Models.Session>(session.Id.ToString())).Returns(session);

            // When
            var result = sessionStore.TryLoadSession(BuildRequestWithSession(session.Id), out items);

            // Then
            result.ShouldBeTrue();
            A.CallTo(() => documentSession.Load<Models.Session>(session.Id.ToString())).MustHaveHappened();
        }

        [Fact]
        public void TryLoad_should_set_items_collection_when_loaded()
        {
            // Given
            IDictionary<string, object> items;
            IDictionary<string, object> sessionItems = new Dictionary<string, object> { { "test", "value" } };
            var session = new Models.Session { Id = Guid.NewGuid(), Items = sessionItems };
            A.CallTo(() => documentSession.Load<Models.Session>(session.Id.ToString())).Returns(session);

            // When
            sessionStore.TryLoadSession(BuildRequestWithSession(session.Id), out items);

            // Then
            items.Count.ShouldEqual(1);
            items["test"].ShouldEqual("value");
            A.CallTo(() => documentSession.Load<Models.Session>(session.Id.ToString())).MustHaveHappened();
        }

        private Request BuildRequestWithSession(Guid sessionId)
        {
            var request = new Request("GET", "/", "http");

            var encryptedId = this.cryptographyConfiguration.EncryptionProvider.Encrypt(sessionId.ToString());
            var hmacBytes = this.cryptographyConfiguration.HmacProvider.GenerateHmac(encryptedId);

            request.Cookies.Add(RavenDbSessionStore.CookieName, String.Format("{0}{1}", Convert.ToBase64String(hmacBytes), encryptedId));
            return request;
        }
    }
}

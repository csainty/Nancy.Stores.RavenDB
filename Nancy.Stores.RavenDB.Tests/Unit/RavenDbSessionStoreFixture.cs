namespace Nancy.Stores.RavenDB.Tests.Unit
{
    using System;
    using FakeItEasy;
    using Nancy.Cryptography;
    using Should;
    using Xunit;

    public class RavenDbSessionStoreFixture
    {
        [Fact]
        public void Should_throw_argumentnullexception_when_no_documentstore_given() {
            // Given
            var result = Record.Exception(() =>
            {
                var store = new RavenDbSessionStore(A.Fake<CryptographyConfiguration>(), null);
            });

            // Then
            result.ShouldBeType<ArgumentNullException>();
        }
    }
}

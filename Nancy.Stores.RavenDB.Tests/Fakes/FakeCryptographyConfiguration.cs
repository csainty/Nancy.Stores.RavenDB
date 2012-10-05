namespace Nancy.Stores.RavenDB.Tests.Fakes
{
    using Cryptography;

    public static class FakeCryptographyConfiguration
    {
        public static CryptographyConfiguration Configuration = new CryptographyConfiguration(new FakeEncryptionProvider(), new FakeHmacProvider());
    }
}

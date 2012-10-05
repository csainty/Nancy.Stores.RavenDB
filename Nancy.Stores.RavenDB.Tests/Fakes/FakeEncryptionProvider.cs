namespace Nancy.Stores.RavenDB.Tests.Fakes
{
    using Cryptography;

    public class FakeEncryptionProvider : IEncryptionProvider
    {
        public string Decrypt(string data)
        {
            return data;
        }

        public string Encrypt(string data)
        {
            return data;
        }
    }
}

namespace Nancy.Stores.RavenDB.Tests.Fakes
{
    using Cryptography;

    public class FakeHmacProvider : IHmacProvider
    {
        public byte[] GenerateHmac(byte[] data)
        {
            return new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        }

        public byte[] GenerateHmac(string data)
        {
            return new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        }

        public int HmacLength
        {
            get { return 8; }
        }
    }
}

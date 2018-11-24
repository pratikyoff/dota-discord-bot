using Bot.Contracts;
using Bot.Universal;
using Xunit;

namespace BotFixture
{
    public class CrypterFixture
    {
        [Fact]
        public void EncryptionAndDecryptionFixture()
        {
            string example = "hello";
            ICrypter crypter = new Crypter();
            var encrypted = crypter.Encrypt(example);
            Assert.NotEqual(encrypted, example);
            Assert.Equal("ifmmp", encrypted);
            var decrypted = crypter.Decrypt(encrypted);
            Assert.Equal(example, decrypted);
        }
    }
}

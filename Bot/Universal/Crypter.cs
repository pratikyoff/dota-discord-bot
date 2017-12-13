using Bot.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Universal
{
    public class Crypter : ICrypter
    {
        public string Encrypt(string toEncrypt)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(toEncrypt));
        }

        public string Decrypt(string toDecrypt)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(toDecrypt));
        }
    }
}

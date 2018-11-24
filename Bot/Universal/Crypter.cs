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
            return ShiftCharsBy(toEncrypt, 1);
        }

        public string Decrypt(string toDecrypt)
        {
            return ShiftCharsBy(toDecrypt, -1);
        }

        private static string ShiftCharsBy(string toEncrypt, int shift)
        {
            char[] encrypted = new char[toEncrypt.Length];
            for (int i = 0; i < toEncrypt.Length; i++)
            {
                encrypted[i] = ((char)(toEncrypt[i] + shift));
            }

            return new string(encrypted);
        }
    }
}

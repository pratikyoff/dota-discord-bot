using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Contracts
{
    public interface ICrypter
    {
        string Encrypt(string toEncrypt);
        string Decrypt(string toDecrypt);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Lazada.Model.Queue
{
    public static class MessageUtils
    {
        public static string GetSignature(byte[] message, string secret)
        {
            HMACSHA256 hmac = new(Encoding.ASCII.GetBytes(secret));
            return Convert.ToHexString(hmac.ComputeHash(message)).ToLower();
        }
    }
}

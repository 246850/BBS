using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BBS.Framework.Helpers
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public sealed class Md5Helper
    {
        private const string Salt = "(*IK<9ol.";
        public static string Encrypt(string source)
        {
            return Encrypt(source, Encoding.UTF8);
        }
        public static string Encrypt(string source, Encoding encoding)
        {
            byte[] byteArray = encoding.GetBytes(string.Concat(source, Salt));
            using (HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider())
            {
                byteArray = hashAlgorithm.ComputeHash(byteArray);
                StringBuilder stringBuilder = new StringBuilder(256);
                foreach (byte item in byteArray)
                    stringBuilder.AppendFormat("{0:x2}", item);
                hashAlgorithm.Clear();
                return stringBuilder.ToString().ToUpper();
            }
        }
    }
}

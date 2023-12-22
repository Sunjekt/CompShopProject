using System;
using System.Security.Cryptography;
using System.Text;

namespace CompShopProject.HashGenerators
{
    public static class MD5Generator
    {
        public static string ProduceMD5Hash(string sourceStr)
        {
            string hash = null;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(sourceStr);

                byte[] hashBytes = md5Hash.ComputeHash(sourceBytes);

                hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }

            return hash;
        }
    }
}

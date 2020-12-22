using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;

namespace AZTU_Akademik.Classes
{
    class HashSHA512
    {
        public static string ComputeSha512Hash(string rawData)
        {
            // Create a SHA256   
            using SHA512 sha512Hash = SHA512.Create();
            // ComputeHash - returns byte array  
            byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}

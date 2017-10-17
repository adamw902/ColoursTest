﻿using System.Security.Cryptography;

namespace ColoursTest.Web.Common
{
    public class RsaKeyHelper
    {
        public static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                return key.ExportParameters(true);
            }
        }
    }
}
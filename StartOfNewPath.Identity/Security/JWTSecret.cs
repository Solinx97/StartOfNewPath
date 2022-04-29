﻿using System;
using System.Security.Cryptography;

namespace StartOfNewPath.Identity.Security
{
    public static class JWTSecret
    {
        public static string SecretKey { get; private set; }

        public static void GenerateSecretKey()
        {
            using var rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[32];
            rng.GetBytes(tokenData);

            SecretKey = Convert.ToBase64String(tokenData);
        }
    }
}

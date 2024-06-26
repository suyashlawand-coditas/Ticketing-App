﻿using TicketingSystem.Core.ServiceContracts;
using System.Security.Cryptography;
using System.Text;

namespace TicketingSystem.Core.Services;

public class CryptoService : ICryptoService
{
    public string Encrypt(string plainText, out string salt)
    {
        // Generate a random salt
        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        salt = Convert.ToBase64String(saltBytes);

        // Convert plaintext to byte array
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        // Concatenate plaintext bytes and salt bytes
        byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];
        Array.Copy(plainTextBytes, plainTextWithSaltBytes, plainTextBytes.Length);
        Array.Copy(saltBytes, 0, plainTextWithSaltBytes, plainTextBytes.Length, saltBytes.Length);

        // Compute hash
        byte[] hashBytes;
        using (var sha256 = SHA256.Create())
        {
            hashBytes = sha256.ComputeHash(plainTextWithSaltBytes);
        }

        // Convert hash to base64 string
        string hash = Convert.ToBase64String(hashBytes);

        return hash;
    }

    public string GenerateSHA256Hash(string plainText)
    {
        // Create a SHA256 hash algorithm instance
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // Compute the hash from the input string
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

            // Convert the byte array to a hexadecimal string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public bool Verify(string plainText, string hash, string salt)
    {
        // Convert plaintext and salt to byte arrays
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] saltBytes = Convert.FromBase64String(salt);

        // Concatenate plaintext bytes and salt bytes
        byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];
        Array.Copy(plainTextBytes, plainTextWithSaltBytes, plainTextBytes.Length);
        Array.Copy(saltBytes, 0, plainTextWithSaltBytes, plainTextBytes.Length, saltBytes.Length);

        // Compute hash 
        byte[] hashBytes;
        using (var sha256 = SHA256.Create())
        {
            hashBytes = sha256.ComputeHash(plainTextWithSaltBytes);
        }

        // Convert hash to base64 string
        string computedHash = Convert.ToBase64String(hashBytes);

        // Compare computed hash with provided hash
        return computedHash == hash;
    }


}
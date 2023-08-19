using System;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        string inputFile = "picture.jpg";
        string outputFile = "encrypted_picture.jpg";
        string keyFilePath = "encryption_key.txt";

        // Generate a random encryption key and save it to a file
        byte[] encryptionKey = GenerateRandomKey();
        File.WriteAllBytes(keyFilePath, encryptionKey);

        // Encrypt the picture
        EncryptFile(inputFile, outputFile, encryptionKey);

        Console.WriteLine("Picture encrypted successfully!");
    }

    static byte[] GenerateRandomKey()
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.GenerateKey();
            return aesAlg.Key;
        }
    }

    static void EncryptFile(string inputFile, string outputFile, byte[] key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;

            using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open))
            using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create))
            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
            using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cryptoStream.Write(buffer, 0, bytesRead);
                }
            }
        }
    }
}

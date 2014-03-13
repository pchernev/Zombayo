using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using System.Threading;
using System.Security.Cryptography;
using System;

namespace Assets.Scripts
{
    public static class SaveLoadGame
    {
        static readonly string PasswordHash = "1co,Pl@men,k1ci,c3c0";
        static readonly string salt = "z0Mb@!Oz0Mb@!O";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        static readonly string _fullPath =  Path.Combine(Application.dataPath, "data.dat");
        public static void SaveStats(Statistics stats)
        {
            using (var stream = new MemoryStream())
            {
                XmlSerializer xs = new XmlSerializer(stats.GetType());
                xs.Serialize(stream, stats);
                var bytes = stream.GetBuffer();
                string str = Encoding.UTF8.GetString(bytes);
                var sb = new StringBuilder(Encrypt(str));
                File.WriteAllText(_fullPath, sb.ToString());
            }
            Debug.Log("File saved...");
        }

        public static Statistics LoadStats()
        {
            if (File.Exists(_fullPath))
            {
                string str = File.ReadAllText(_fullPath);
                var sb = new StringBuilder(Decrypt(str));
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Statistics));
                    Debug.Log("File loaded ...");
                    return (Statistics)xs.Deserialize(ms);
                }
            }
            var stats = new Statistics() { };
            SaveStats(stats);
            return new Statistics { Coins = 0, Distance = 0, Points = 0 };
        }

        private static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(salt)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        private static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(salt)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}
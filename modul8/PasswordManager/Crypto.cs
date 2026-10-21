using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager
{
    internal static class Crypto
    {
        public static string NewSalt()
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(16);
            return Convert.ToBase64String(bytes); // преобразование в текст
        }
        // Хэш пароля 
        public static string Hash(string password, string salt)
        { // получение спец значений из пароля
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password, Convert.FromBase64String(salt),
                100000, HashAlgorithmName.SHA256, 32);
            return Convert.ToBase64String(hash);
        }
        // Ключ для шифрования паролей 
        public static byte[] MakeKey(string password, string keySalt)
        {
            return Rfc2898DeriveBytes.Pbkdf2(
                password, Convert.FromBase64String(keySalt),
                100000, HashAlgorithmName.SHA256, 32);
        }
        // Шифрование сохраненного пароля
        public static string Encrypt(string text, byte[] key, string iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key; // установка ключа, используемого для шифрования
                aes.IV = Convert.FromBase64String(iv);
                byte[] data = Encoding.UTF8.GetBytes(text); // преобразование текста в байты
                byte[] result = aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(result); // создает обьект, 
            }
        }
        // Расшифровка AES
        public static string Decrypt(string encrypted, byte[] key, string iv)
        {
            using (Aes aes = Aes.Create()) // установка параметров
            {
                aes.Key = key;
                aes.IV = Convert.FromBase64String(iv);
                byte[] data = Convert.FromBase64String(encrypted); // преобразование зашифрованной строки в байты
                byte[] result = aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length); // расшифровка
                return Encoding.UTF8.GetString(result);
            }
        }

    }
}
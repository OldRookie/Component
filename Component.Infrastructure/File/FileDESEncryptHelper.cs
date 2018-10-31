using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure
{
    public class FileDESEncryptHelper
    {
        const string crypt_key = "abc123";

        public static byte[] CryptBytesToBytes(byte[] in_date, bool encrypt)
        {
            TripleDESCryptoServiceProvider des_provider = new TripleDESCryptoServiceProvider();
            int key_size_bits = 0;
            for (int i = 1024; i > 1; i--)
            {
                if (des_provider.ValidKeySize(i))
                {
                    key_size_bits = i;
                    break;
                }
            }
            int block_size_bits = des_provider.BlockSize;
            byte[] key = null;
            byte[] iv = null;
            MakeKeyAndIV(crypt_key, key_size_bits, block_size_bits, ref key, ref iv);
            ICryptoTransform crypto_transform;
            if (encrypt)
                crypto_transform = des_provider.CreateEncryptor(key, iv);
            else
                crypto_transform = des_provider.CreateDecryptor(key, iv);
            byte[] b;
            MemoryStream out_stream = new MemoryStream();
            CryptoStream crypto_stream = new CryptoStream(out_stream, crypto_transform, CryptoStreamMode.Write);
            StreamWriter streamWriter = new StreamWriter(crypto_stream);
            crypto_stream.Write(in_date, 0, in_date.Length);
            //b = out_stream.ToArray();
            b = crypto_transform.TransformFinalBlock(in_date, 0, in_date.Length);

            return b;
        }

        public static Stream CryptStream(Stream fileStream, bool encrypt)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //ms.Position = 0;
                fileStream.CopyTo(ms);
                var cryptBytes = CryptBytesToBytes(ms.ToArray(), encrypt);
                return new MemoryStream(cryptBytes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passwork"></param>
        /// <param name="key_size_bits"></param>
        /// <param name="block_size_bits"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        static void MakeKeyAndIV(string passwork, int key_size_bits, int block_size_bits, ref byte[] key, ref byte[] iv)
        {
            PasswordDeriveBytes password_derive_bytes = new PasswordDeriveBytes(crypt_key, null, "SHA384", 1000);
            key = password_derive_bytes.GetBytes(key_size_bits / 8);
            iv = password_derive_bytes.GetBytes(block_size_bits / 8);
        }

        /// <summary>
        /// 加密/解密文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public byte[] CryptFileToBytes(string filePath, bool encrypt)
        {
            TripleDESCryptoServiceProvider des_provider = new TripleDESCryptoServiceProvider(); ;
            int key_size_bits = 0;
            for (int i = 1024; i > 1; i--)
            {
                if (des_provider.ValidKeySize(i))
                {
                    key_size_bits = i;
                    break;
                }
            }

            int block_size_bits = des_provider.BlockSize;
            byte[] key = null;
            byte[] iv = null;
            MakeKeyAndIV(crypt_key, key_size_bits, block_size_bits, ref key, ref iv);

            ICryptoTransform crypto_transform;
            if (encrypt)
            {
                crypto_transform = des_provider.CreateEncryptor(key, iv);
            }
            else
            {
                crypto_transform = des_provider.CreateDecryptor(key, iv);
            }
            byte[] b_emptyp = new byte[0];
            try
            {
                byte[] b;
                //using()
                //{
                MemoryStream out_stream = new MemoryStream();
                FileStream in_stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                CryptoStream crypto_stream = new CryptoStream(out_stream, crypto_transform, CryptoStreamMode.Write);
                StreamWriter streamWriter = new StreamWriter(crypto_stream);
                const int BLOCK_SIZE = 1024;
                byte[] buffer = new byte[BLOCK_SIZE];
                int bytes_read;
                do
                {
                    bytes_read = in_stream.Read(buffer, 0, BLOCK_SIZE);
                    if (bytes_read == 0)
                        break;
                    crypto_stream.Write(buffer, 0, bytes_read);
                } while (true);
                //}
                b = out_stream.ToArray();
                out_stream.Dispose();
                //crypto_stream.Dispose();
                //streamWriter.Dispose();
                in_stream.Dispose();

                return b;

            }
            catch (Exception ex)
            {
                // return b_emptyp;
            }
            return b_emptyp;
        }

        public static byte[] Encrypt(byte[] source, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            byte[] byteHash;
            byte[] byteBuff;

            byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
            byteBuff = source;

            var encoded = desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length);
            return encoded;
        }

        public static byte[] Decrypt(byte[] encodedBytes, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            byte[] byteHash;
            byte[] byteBuff;

            byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
            byteBuff = encodedBytes;

            var plaintext = desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length);
            return plaintext;
        }

        public string Encrypt(string source, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            byte[] byteHash;
            byte[] byteBuff;

            byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
            byteBuff = Encoding.UTF8.GetBytes(source);

            string encoded =
                Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return encoded;
        }

        public static string Decrypt(string encodedText, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            byte[] byteHash;
            byte[] byteBuff;

            byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
            byteBuff = Convert.FromBase64String(encodedText);

            string plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return plaintext;
        }
    }

}

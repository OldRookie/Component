using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure.Security
{
    public class X509CertificationHelper {
        #region Find cert
        /// <summary>
        /// Open Cert File 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static X509Certificate2 Open(string FileName, string Password)
        {
            X509Certificate2 x509 = new X509Certificate2(FileName, Password, X509KeyStorageFlags.Exportable);
            return x509;
        }


        /// <summary>
        /// Search Cert from Environment
        /// </summary>
        /// <param name="certThumbPrint"></param>
        /// <returns></returns>
        public static X509Certificate2 Search(string certThumbPrint)
        {
            // search from local machine  storage
            X509Certificate2 cert = FindFromLocalMachineLocation(certThumbPrint);

            if (cert != null)
            {
                return cert;
            }

            // search from current user storage
            cert = FindFromCurrentUserLocation(certThumbPrint);
            if (cert != null)
            {
                return cert;
            }

            return null;
        }

        /// <summary>
        ///  Search cert form Current User Location of OS
        /// </summary>
        /// <param name="strThumbprint"></param>
        /// <param name="storeName"></param>
        /// <param name="storeLocation"></param>
        /// <returns></returns>
        public static X509Certificate2 FindFromCurrentUserLocation(string strThumbprint, StoreName storeName = StoreName.My)
        {
            try
            {
                X509Store store = new X509Store(storeName, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                X509Certificate2Collection storecollection = store.Certificates;

                foreach (X509Certificate2 x509 in storecollection)
                {
                    string x509Thumbprint = x509.Thumbprint;

                    if (x509Thumbprint.Equals(strThumbprint, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return x509;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        ///  Search cert form local Machine Location of OS
        /// </summary>
        /// <param name="strThumbprint"></param>
        /// <param name="storeName"></param>
        /// <param name="storeLocation"></param>
        /// <returns></returns>
        public static X509Certificate2 FindFromLocalMachineLocation(string strThumbprint, StoreName storeName = StoreName.My)
        {

            try
            {
                X509Store store = new X509Store(storeName, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                X509Certificate2Collection storecollection = store.Certificates;

                foreach (X509Certificate2 x509 in storecollection)
                {
                    string x509Thumbprint = x509.Thumbprint;

                    if (x509Thumbprint.Equals(strThumbprint, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return x509;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        #endregion

        #region Application

        /// <summary>
        /// Get Public Key
        /// </summary>
        /// <param name="certification"></param>
        /// <returns></returns>
        public static string GetPublicKey(X509Certificate2 certification)
        {
            return certification.PublicKey.Key.ToXmlString(false);
        }

        /// <summary>
        /// Get Private Key
        /// </summary>
        /// <param name="certification"></param>
        /// <returns></returns>
        public static string GetPrivateKey(X509Certificate2 certification)
        {
            return certification.PrivateKey.ToXmlString(true);
        }

        /// <summary>
        /// Get Private key from  xml private key File
        /// </summary>
        /// <param name="fileName">The File Path of The Private Key</param>
        /// <returns></returns>
        public static string GetPrivateKey(string fileName = "PrivateKey.xml")
        {
            string privateKeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(privateKeyPath))
            {
                throw new FileNotFoundException("The Private Key File Not Found");
            }
            StreamReader sr = new StreamReader(privateKeyPath, Encoding.GetEncoding("utf-8"));
            string content = sr.ReadToEnd().Trim();
            sr.Close();
            return content;
        }

        #endregion

        #region X509 RSA
        /// <summary>
        /// Encrypt by public Key
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="xmlPublicKey"></param>
        /// <returns></returns>
        public static string Encrypt(string plaintext, string xmlPublicKey)
        {
            AsymmetricAlgorithm _AsymmetricAlgorithm = AsymmetricAlgorithm.Create();
            _AsymmetricAlgorithm.FromXmlString(xmlPublicKey);
            RSACryptoServiceProvider RSACryptography = (RSACryptoServiceProvider)_AsymmetricAlgorithm;

            Byte[] PlaintextData = Encoding.UTF8.GetBytes(plaintext);
            int MaxBlockSize = RSACryptography.KeySize / 8 - 11;    //加密块最大长度限制

            if (PlaintextData.Length <= MaxBlockSize)
                return Convert.ToBase64String(RSACryptography.Encrypt(PlaintextData, false));

            using (MemoryStream PlaiStream = new MemoryStream(PlaintextData))
            using (MemoryStream CrypStream = new MemoryStream())
            {
                Byte[] Buffer = new Byte[MaxBlockSize];
                int BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                while (BlockSize > 0)
                {
                    Byte[] ToEncrypt = new Byte[BlockSize];
                    Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);


                    Byte[] Cryptograph = RSACryptography.Encrypt(ToEncrypt, false);
                    CrypStream.Write(Cryptograph, 0, Cryptograph.Length);
                    BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                }
                return Convert.ToBase64String(CrypStream.ToArray(), Base64FormattingOptions.None);
            }

        }

        /// <summary>
        /// Decoded by the Private Key
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="xmlPrivateKey"></param>
        /// <returns></returns>

        public static string Decrypt(string ciphertext, string xmlPrivateKey)
        {
            AsymmetricAlgorithm _AsymmetricAlgorithm = AsymmetricAlgorithm.Create();
            _AsymmetricAlgorithm.FromXmlString(xmlPrivateKey);
            RSACryptoServiceProvider RSACryptography = (RSACryptoServiceProvider)_AsymmetricAlgorithm;

            Byte[] CiphertextData = Convert.FromBase64String(ciphertext);
            int MaxBlockSize = RSACryptography.KeySize / 8;    //解密块最大长度限制

            if (CiphertextData.Length <= MaxBlockSize)
            {
                byte[] res = RSACryptography.Decrypt(CiphertextData, false);
                return Encoding.UTF8.GetString(res);
            }

            using (MemoryStream CrypStream = new MemoryStream(CiphertextData))
            using (MemoryStream PlaiStream = new MemoryStream())
            {
                Byte[] Buffer = new Byte[MaxBlockSize];
                int BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                while (BlockSize > 0)
                {
                    Byte[] ToDecrypt = new Byte[BlockSize];
                    Array.Copy(Buffer, 0, ToDecrypt, 0, BlockSize);
                    Byte[] Plaintext = RSACryptography.Decrypt(ToDecrypt, false);
                    PlaiStream.Write(Plaintext, 0, Plaintext.Length);
                    BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                }
                return Encoding.UTF8.GetString(PlaiStream.ToArray());
            }

        }
        #endregion
    }
}

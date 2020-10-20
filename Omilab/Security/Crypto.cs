using System;
using System.Text;
using System.Security.Cryptography;


namespace Omilab.Security
{
    public static class Crypto
    {    

        /// <summary>
        /// Encrypt the given string using the specified key.
        /// </summary>
        /// <param name="decryptedText">The string to be encrypted.</param>
        /// <param name="secret">The encryption password key.</param>
        /// <returns>The encrypted string.</returns>
        public static string Encrypt(string decryptedText, string secret)
        {
            try
            {
                TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();

                byte[] byteHash, byteBuff;
                string strTempKey = secret;

                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB

                byteBuff = ASCIIEncoding.ASCII.GetBytes(decryptedText);
                return Convert.ToBase64String(objDESCrypto.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Decrypt the given string using the specified key.
        /// </summary>
        /// <param name="encryptedText">The string to be decrypted.</param>
        /// <param name="secret">The encryption password key.</param>
        /// <returns>The decrypted string.</returns>
        public static string Decrypt(string encryptedText, string secret)
        {
            try
            {
                TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();

                byte[] byteHash, byteBuff;
                string strTempKey = secret;

                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB

                byteBuff = Convert.FromBase64String(encryptedText);
                string strDecrypted = ASCIIEncoding.ASCII.GetString(objDESCrypto.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                objDESCrypto = null;

                return strDecrypted;
            }
            catch (Exception ex)
            {
                //Error.LogException(ex);
                //return "Wrong Input. " + ex.Message;
                return ex.Message;
            }
        }

    } //end class
} //end namespace

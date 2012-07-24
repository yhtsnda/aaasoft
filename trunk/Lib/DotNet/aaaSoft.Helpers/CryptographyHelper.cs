using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace aaaSoft.Helpers
{
    public class CryptographyHelper
    {
        public static String ComputeMD5Hash(String data)
        {
            var buffer = ComputeMD5Hash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(buffer).Replace("-", "");
        }

        public static byte[] ComputeMD5Hash(byte[] data)
        {
            var md5 = MD5.Create();
            return md5.ComputeHash(data);
        }

        public static byte[] ComputeMD5Hash(Stream stream)
        {
            var md5 = MD5.Create();
            return md5.ComputeHash(stream);
        }

        public static byte[] ComputeMD5Hash(FileInfo fileInfo)
        {
            Stream localFileStream = null;
            Exception exception = null;
            try
            {
                localFileStream = fileInfo.OpenRead();
                return CryptographyHelper.ComputeMD5Hash(localFileStream);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                try { localFileStream.Close(); }
                catch { }
            }
            throw exception;
        }

        public static byte[] DesEncrypt(byte[] data, byte[] password)
        {
            var des = DES.Create();
            var enc = des.CreateEncryptor(password, password);
            return enc.TransformFinalBlock(data, 0, data.Length);
        }

        public static byte[] DesDecrypt(byte[] data, byte[] password)
        {
            var des = DES.Create();
            var dec = des.CreateDecryptor(password, password);
            return dec.TransformFinalBlock(data, 0, data.Length);
        }

        private static byte[] GetDesPassword(String password)
        {
            var pwdMd5 = ComputeMD5Hash(Encoding.UTF8.GetBytes(password));
            var pwdBuffer = new Byte[8];
            for (int i = 0; i < 8; i++)
            {
                pwdBuffer[i] = pwdMd5[i];
            }
            return pwdBuffer;
        }

        public static String DesEncrypt(String data, String password)
        {
            var dataBuffer = Encoding.UTF8.GetBytes(data);
            var pwdBuffer = GetDesPassword(password);
            return Convert.ToBase64String(DesEncrypt(dataBuffer, pwdBuffer));
        }

        public static String DesDecrypt(String data, String password)
        {
            var dataBuffer = Convert.FromBase64String(data);
            var pwdBuffer = GetDesPassword(password);
            return Encoding.UTF8.GetString(DesDecrypt(dataBuffer, pwdBuffer));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace aaaSoft.Update.Helpers
{
    class CompressHelper
    {
        //压缩文件
        public static void CompressFile(String srcFileName, String desFileName)
        {
            FileStream srcStream = new FileStream(srcFileName, FileMode.Open);
            FileStream desStream = new FileStream(desFileName, FileMode.OpenOrCreate, FileAccess.Write);
            CompressStream(srcStream, desStream);
        }

        /// <summary>
        /// 压缩到流
        /// </summary>
        /// <param name="srcStream"></param>
        /// <param name="desStream"></param>
        public static void CompressStream(Stream srcStream, Stream desStream)
        {
            desStream.SetLength(0);
            GZipStream Gz = new GZipStream(desStream, CompressionMode.Compress, false);

            Byte[] newBytes = new Byte[1000];
            Int32 rtnCount = 0;
            do
            {
                rtnCount = srcStream.Read(newBytes, 0, newBytes.Length);
                Gz.Write(newBytes, 0, rtnCount);
            }
            while (rtnCount == newBytes.Length);

            srcStream.Close();
            Gz.Flush();
            Gz.Close();
        }
        //解压文件
        public static void DecompressFile(String srcFileName, String desFileName)
        {
            FileStream srcStream = new FileStream(srcFileName, FileMode.Open);
            FileStream desStream = new FileStream(desFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            DecompressStream(srcStream, desStream);
        }

        /// <summary>
        /// 从流中解压
        /// </summary>
        /// <param name="srcStream"></param>
        /// <param name="desStream"></param>
        public static void DecompressStream(Stream srcStream, Stream desStream)
        {
            Exception throwEx = null;
            try
            {
                desStream.SetLength(0);
                GZipStream Gz = new GZipStream(srcStream, CompressionMode.Decompress, false);

                Byte[] newBytes = new Byte[1000];

                Int32 rtnCount = 0;
                do
                {
                    rtnCount = Gz.Read(newBytes, 0, newBytes.Length);
                    desStream.Write(newBytes, 0, rtnCount);
                }
                while (rtnCount == newBytes.Length);
            }
            catch (Exception ex)
            {
                throwEx = ex;
            }
            finally
            {
                srcStream.Close();
                desStream.Close();
            }
            if (throwEx != null)
            {
                throw throwEx;
            }
        }

        /// <summary>
        /// 从字节数组中解压
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <returns></returns>
        public static Byte[] DecompressBytes(Byte[] srcBytes)
        {
            MemoryStream srcStream = new MemoryStream(srcBytes);
            MemoryStream desStream = new MemoryStream();
            DecompressStream(srcStream, desStream);
            return desStream.ToArray();
        }
    }
}

using SevenZip;
using System;

namespace SudoItApi
{
    public class SevenZipApi
    {
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="FromPath">压缩文件路径</param>
        /// <param name="ToPath">解压路径</param>
        public static void Unzip(string FromPath,string ToPath)
        {
            SevenZipExtractor tmp = new SevenZipExtractor(FromPath);
            {
                for (int i = 0; i < tmp.ArchiveFileData.Count; i++)
                {
                    tmp.ExtractFiles(ToPath, tmp.ArchiveFileData[i].Index);
                }
            }
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="FromPath">所需压缩的文件路径</param>
        /// <param name="ToPath">压缩文件路径</param>
        /// <param name="Format">格式</param>
        public static void ZipFiles(string FromPath, string ToPath, string Format)
        {
            SevenZipCompressor tmp = new SevenZipCompressor();
            switch (Format)
            {
                case "SevenZip":
                case "7z":
                    tmp.ArchiveFormat = OutArchiveFormat.SevenZip;
                    break;
                case "tar":
                case "Tar":
                    tmp.ArchiveFormat = OutArchiveFormat.Tar;
                    break;
                case "Zip":
                case "zip":
                    tmp.ArchiveFormat = OutArchiveFormat.Zip;
                    break;
                default:
                    Exception ex = new Exception("Cannot detect file type. Available type: 7z, Tar, Zip.");
                    throw ex;
            }
            tmp.CompressFiles(ToPath, FromPath);
        }
        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="FromPath">所需压缩的目录路径</param>
        /// <param name="ToPath">压缩文件路径</param>
        /// <param name="Format">格式</param>
        public static void ZipDir(string FromPath, string ToPath, string Format)
        {
            SevenZipCompressor tmp = new SevenZipCompressor();
            switch (Format)
            {
                case "SevenZip":
                case "7z":
                    tmp.ArchiveFormat = OutArchiveFormat.SevenZip;
                    break;
                case "tar":
                case "Tar":
                    tmp.ArchiveFormat = OutArchiveFormat.Tar;
                    break;
                case "Zip":
                case "zip":
                    tmp.ArchiveFormat = OutArchiveFormat.Zip;
                    break;
                default:
                    Exception ex = new Exception("Cannot detect file type. Available type: 7z, Tar, Zip.");
                    throw ex;
            }
            tmp.CompressDirectory(FromPath, ToPath);
        }
    }
}

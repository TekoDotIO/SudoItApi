using SevenZip;
using System;

namespace SudoItApi
{
    /// <summary>
    /// 7z压缩操作类
    /// </summary>
    public class SevenZipApi
    {
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="FromPath">压缩文件路径</param>
        /// <param name="ToPath">解压路径</param>
        public static void Unzip(string FromPath,string ToPath)
        {
            if (IntPtr.Size == 4)
            {
                SevenZipBase.SetLibraryPath(@"./x86/7z.dll");
            }
            else
            {
                SevenZipBase.SetLibraryPath(@"./x64/7z.dll");
            }
            SevenZipExtractor tmp = new SevenZipExtractor(FromPath);
            {
                for (int i = 0; i < tmp.ArchiveFileData.Count; i++)
                {
                    tmp.ExtractFiles(ToPath, tmp.ArchiveFileData[i].Index);//打包文件
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
            if (IntPtr.Size == 4)
            {
                SevenZipBase.SetLibraryPath(@"./x86/7z.dll");
            }
            else
            {
                SevenZipBase.SetLibraryPath(@"./x64/7z.dll");
            }
            SevenZipCompressor tmp = new SevenZipCompressor();//构建压缩类
            switch (Format)//转换格式
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
                    throw ex;//抛异常
            }
            tmp.CompressFiles(ToPath, FromPath);//开始压缩
        }
        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="FromPath">所需压缩的目录路径</param>
        /// <param name="ToPath">压缩文件路径</param>
        /// <param name="Format">格式</param>
        public static void ZipDir(string FromPath, string ToPath, string Format)
        {
            if (IntPtr.Size == 4)
            {
                SevenZipBase.SetLibraryPath(@"./x86/7z.dll");
            }
            else
            {
                SevenZipBase.SetLibraryPath(@"./x64/7z.dll");
            }
            SevenZipCompressor tmp = new SevenZipCompressor();//构建压缩类
            switch (Format)//转换格式
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
            tmp.CompressDirectory(FromPath, ToPath);//开始压缩
        }
    }
}

using Microsoft.AspNetCore.Http;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace VideoDemo.Common
{
    public static class UploadHelper
    {
        public static bool IsAllowedFileType(string fileName)
        {
            var allowedExtensions = new[] { ".mp4",".mkv"}; // 允许的文件扩展名
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            return allowedExtensions.Contains(fileExtension);
        }

        public static bool IsCompressionNeeded(IFormFile file)
        {
            return file.Length > MaxFileSize; // 如果文件大小超过最大值，则需要压缩处理
        }

        private const long MaxFileSize = 1024 * 1024 * 200; // 最大文件大小为200MB

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void CompressFile(string filePath)
        {
            using (var inputFileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var archiveFileStream = new FileStream(filePath + ".zip", FileMode.Create))
                {
                    using (var archive = new ZipArchive(archiveFileStream, ZipArchiveMode.Create))
                    {
                        var archiveEntry = archive.CreateEntry(Path.GetFileName(filePath), CompressionLevel.Optimal);
                        using (var entryStream = archiveEntry.Open())
                        {
                            inputFileStream.CopyTo(entryStream);
                        }
                    }
                }
            }
        }
    }
}

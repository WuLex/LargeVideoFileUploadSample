using Microsoft.AspNetCore.Http;

namespace VideoDemo.Models
{
    public class PlUploadUploadModel
    {
        /// <summary>
        /// 上传的文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 当前块的索引
        /// </summary>
        public int Chunk { get; set; }

        /// <summary>
        /// 文件被分割成的总块数
        /// </summary>
        public int TotalChunks { get; set; }

        /// <summary>
        /// 上传的文件大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 上传的文件
        /// </summary>
        public IFormFile File { get; set; }
    }
}
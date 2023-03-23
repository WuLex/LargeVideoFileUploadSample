using Microsoft.AspNetCore.Http;

namespace VideoDemo.Models
{
    public class PluploadModel
    {
        public string FilePath { get; set; }
        public string Name { get; set; }
        public int Chunk { get; set; }
        public int Chunks { get; set; }
        public long Size { get; set; }
      
        /// <summary>
        /// 上传的文件
        /// </summary>
        public IFormFile File { get; set; }
    }
}

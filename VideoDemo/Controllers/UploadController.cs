using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using VideoDemo.Models;

namespace VideoDemo.Controllers
{

    /// <summary>
    /// 断点续传示例2
    /// </summary>
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;

        public UploadController(ILogger<UploadController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(PluploadModel model)
        {
            // 上传的块文件所在的目录
            var chunkDirectory = Path.Combine(Path.GetTempPath(), "plupload", model.Name);

            // 如果不存在上传的块文件，则返回错误响应
            if (!Directory.Exists(chunkDirectory))
            {
                Directory.CreateDirectory(chunkDirectory);
                //return BadRequest("Chunks not found.");
            }

            // 合并后的文件路径
            var filePath = Path.Combine(Path.GetTempPath(),model.Name);
           
            // 块文件路径拼接
            var chunkPath = Path.Combine(chunkDirectory, model.Chunk.ToString());

            // 如果是第一个块，则创建文件并写入数据
            if (model.Chunk == 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    #region 写入块
                    using (var chunkStream = new FileStream(chunkPath, FileMode.Create, FileAccess.Write))
                    {
                        model.File.CopyTo(chunkStream);
                    }
                    #endregion

                    //读取块
                    using (var chunkStream = new FileStream(chunkPath, FileMode.Open, FileAccess.Read))
                    {
                        chunkStream.CopyTo(stream);
                    }
                }
            }
            else
            {
                // 如果不是第一个块，则追加写入数据
                using (var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    #region 写入块
                    using (var chunkStream = new FileStream(chunkPath, FileMode.Create, FileAccess.Write))
                    {
                        model.File.CopyTo(chunkStream);
                    }
                    #endregion


                    using (var chunkStream = new FileStream(chunkPath, FileMode.Open, FileAccess.Read))
                    {
                        chunkStream.CopyTo(stream);
                    }
                }
            }

            // 如果上传的块数等于总块数，则表示上传完成
            if (model.Chunk + 1 == model.Chunks)
            {
                // 删除上传的块文件
                Directory.Delete(chunkDirectory, true);
                _logger.LogInformation($"File {model.Name} uploaded successfully.");
            }

            return Json(new { status = "OK" });
        }


        /// <summary>
        /// 未使用
        /// </summary>
        /// <param name="chunkDirectory"></param>
        /// <param name="filePath"></param>
        public void MergeFile(string chunkDirectory,string filePath)
        {
            // 将所有块文件按照编号排序
            var chunkFiles = Directory.GetFiles(chunkDirectory)
                .Where(x => Path.GetExtension(x) == ".part")
                .OrderBy(x => int.Parse(Path.GetFileNameWithoutExtension(x)));

            // 将所有块文件按顺序合并到一个完整的文件中
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                foreach (var chunkFile in chunkFiles)
                {
                    var buffer = System.IO.File.ReadAllBytes(chunkFile);
                    fs.Write(buffer, 0, buffer.Length);
                }
            }

        }
    }
}

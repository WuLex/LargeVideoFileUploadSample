﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using VideoDemo.Common;
using VideoDemo.Models;

namespace VideoDemo.Controllers
{
    public class UploadLargeFileController : Controller
    {
        private readonly ILogger<UploadLargeFileController> _logger;

        public UploadLargeFileController(ILogger<UploadLargeFileController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadChunk([FromForm] PlUploadUploadModel model)
        {
            var chunkDirectory = Path.Combine(Path.GetTempPath(), "plupload", model.Name);

            if (!Directory.Exists(chunkDirectory))
            {
                Directory.CreateDirectory(chunkDirectory);
            }

            var chunkPath = Path.Combine(chunkDirectory, model.Chunk.ToString());
         
            // 在拷贝文件内容之前验证上传文件的类型和大小是否符合要求
            if (!UploadHelper.IsAllowedFileType(model.File.FileName))
            {
                return BadRequest("File type not allowed.");
            }

            //if (model.File.Length > MaxFileSize)
            //{
            //    return BadRequest("File size exceeded maximum limit.");
            //}
           
            // 将上传的文件的内容写入到指定的文件流中
            using (var stream = new FileStream(chunkPath, FileMode.Create, FileAccess.Write))
            {
                model.File.CopyTo(stream);
            }

            // 在拷贝文件内容之后对上传的文件进行一些后续处理
            //if (UploadHelper.IsCompressionNeeded(model.File))
            //{
            //    UploadHelper.CompressFile(filePath);
            //}

            _logger.LogInformation($"Chunk {model.Chunk} of {model.TotalChunks} uploaded successfully.");

            return Json(new { status = "OK" });
        }

        [HttpPost]
        public IActionResult UploadFile([FromForm] PlUploadCompleteModel model)
        {
            var chunkDirectory = Path.Combine(Path.GetTempPath(), "plupload", model.Name);
            var filePath = Path.Combine(Path.GetTempPath(), "plupload", model.Name);

            if (!Directory.Exists(chunkDirectory))
            {
                return BadRequest("Chunks not found.");
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                for (var i = 0; i < model.TotalChunks; i++)
                {
                    var chunkPath = Path.Combine(chunkDirectory, i.ToString());

                    if (!System.IO.File.Exists(chunkPath))
                    {
                        return BadRequest($"Chunk {i} is missing.");
                    }

                    using (var chunkStream = new FileStream(chunkPath, FileMode.Open, FileAccess.Read))
                    {
                        chunkStream.CopyTo(stream);
                    }
                }
            }

            _logger.LogInformation($"File {model.Name} uploaded successfully.");

            return Json(new { status = "OK" });
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace VideoDemo.Controllers
{
    public class DifferentResolutionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetVideo(string resolution)
        {
            // 根据请求的分辨率获取对应的视频链接
            string videoUrl = GetVideoUrlByResolution(resolution);

            // 将视频链接返回给前端
            return Json(new { url = videoUrl });
        }

        private string GetVideoUrlByResolution(string resolution)
        {
            // 在此处根据分辨率返回对应的视频链接
            // 这里仅为示例，你需要根据实际情况进行处理
            if (resolution == "1080p")
            {
                return "/uploads/compressed.mp4";
            }
            else if (resolution == "720p")
            {
                return "/uploads/compressed.mp4";
            }
            else
            {
                return "/uploads/compressed.mp4";
            }
        }
    }
}

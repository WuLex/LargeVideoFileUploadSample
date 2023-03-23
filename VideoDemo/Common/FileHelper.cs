using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VideoDemo.Models;

namespace VideoDemo.Common
{
    public class FileHelper
    {
        private static string finalpath = "";

        public static int UploadVideo(VideoFileModel videoFile)
        {
            //前端传输是否为切割文件最后一个小文件
            var isLast = videoFile.isLast;
            //前端传输当前为第几次切割小文件
            var count = videoFile.Count;
            //获取前端处理过的传输文件名
            string fileName = videoFile.Name;
            //存储接受到的切割文件
            if (videoFile.Files.Length <= 0)
            {
                return -1;
            }

            IFormFile formFile = videoFile.Files;

            //处理文件名称(去除.part*，还原真实文件名称)
            string newFileName = fileName.Substring(0, fileName.LastIndexOf('.'));

            //临时存储文件夹路径
            var desPath = $"{Directory.GetCurrentDirectory()}/uploads/slice/{newFileName}";
            //判断指定目录是否存在临时存储文件夹，没有就创建
            if (!System.IO.Directory.Exists(desPath))
            {
                //不存在就创建目录
                System.IO.Directory.CreateDirectory(desPath);
            }

            //存储文件
            using (var stream = new FileStream(desPath + "/" + fileName, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }
            //file.SaveAs("E:\\uploads\\slice\\" + newFileName + "\\" + fileName);

            //判断是否为最后一次切割文件传输
            if (isLast == "true")
            {
                //判断组合的文件是否存在
                //finalpath = Directory.GetCurrentDirectory() + @"/uploads/" + newFileName;
                finalpath = $"{Directory.GetCurrentDirectory()}/uploads/{newFileName}";
                if (File.Exists(finalpath)) //如果文件存在
                {
                    File.Delete(finalpath); //先删除,否则新文件就不能创建
                }

                //创建空的文件流
                using (var fileOut = new FileStream(finalpath, FileMode.CreateNew, FileAccess.ReadWrite))
                using (var bw = new BinaryWriter(fileOut))
                {
                    //获取临时存储目录下的所有切割文件
                    string[] allFile = Directory.GetFiles(desPath);
                    //将文件进行排序拼接
                    allFile = allFile.OrderBy(s => int.Parse(Regex.Match(s, @"\d+$").Value)).ToArray();
                    for (int i = 0; i < allFile.Length; i++)
                    {
                        using (var fileIn = new FileStream(allFile[i], FileMode.Open))
                        using (var br = new BinaryReader(fileIn))
                        {
                            byte[] data = new byte[1048576]; //流读取,缓存空间
                            int readLen = 0; //每次实际读取的字节大小
                            readLen = br.Read(data, 0, data.Length);
                            bw.Write(data, 0, readLen);
                            //关闭输入流
                            //fileIn.Close();
                        }
                    }
                    //关闭二进制写入
                    //bw.Close();
                    //FileOut.Close();
                }
                ClipVideo();
            }

            return int.Parse(count) + 1;
        }

        public static void ClipVideo()
        {
            /**
             * 支持视频格式：mpeg，mpg，avi，dat，mkv，rmvb，rm，mov.
             *不支持：wmv
             * **/
            //FFmpeg.SetExecutablesPath("ffmpeg.exe");
            //ffmpeg.exe的路径，控制台程序会在执行目录（....FFmpeg测试\bin\Debug）下找此文件，

            //视频路径
            string finalname = Path.GetFileNameWithoutExtension(finalpath);
            string videoFilePath = finalpath; //"d:\\01.avi";
            finalpath = finalpath.Replace(finalname, finalname + "15S");

            #region 自定义逻辑
            //IMediaInfo videoFile = await FFmpeg.GetMediaInfo(videoFilePath);
            //TimeSpan totaotp = videoFile.Duration;
            //string totalTime = string.Format("{0:00}:{1:00}:{2:00}", (int)totaotp.TotalHours, totaotp.Minutes,totaotp.Seconds);



            //string sourceFileName = Path.GetFileName(upFile.get_FileName()); //取出上传的视频的文件名，进而取出该文件的扩展名
            //string sourceFileName = "02.avi";
            //string flv_file = System.IO.Path.ChangeExtension("d:\\01.avi", ".flv");
            //string Command = " -i \"" + FromName + "\" -y -ab 32 -ar 22050 -b 800000 -s  480*360 \"" + ExportName + "\""; //Flv格式

            //转换视频为flv
            //ffmpeg -i F:\01.wmv -ab 56 -ar 22050 -b 500 -r 15 -s 320x240 f:\test.flv

            //视频截图,fileName视频地址,imgFile图片地址
            //ffmpeg -i input.flv -y -f image2 -ss 10.11 -t 0.001 -s 240x180 catchimg.jpg;
            //ImgstartInfo.Arguments = "   -i   " + fileName + "  -y  -f  image2   -ss 2 -vframes 1  -s   " + FlvImgSize + "   " + flv_img;

            //string Command = " -i \"test.wmv\" -y -ab 32 -ar 22050 -b 800000 -s 320*240 \"2.flv\"";
            //string Command = "E:\\FFmpeg\\ffmpeg.exe -i E:\\ClibDemo\\VideoPath\\admin\\a.wmv -y -ab 56 -ar 22050 -b 500 -r 15 -s 320*240 " ExportName;

            //3.重新编码进行剪切
            //ffmpeg -ss [start] -t [duration] -i [in].mp4  -c:v libx264 -c:a aac -strict experimental -b:a 98k [out].mp4
            //相对来说比较精确，可是还是不是特别精确

            //string Command =" -ss 00:00:00 -t 00:00:15 -i  d:\\01.avi   d:\\output.avi"; // ffmpeg -ss 00:00:10 -t 00:01:22 -i 五月天-突然好想你.mp3  out.mp3
            string Command = $"-ss 00:00:00 -t 00:00:15 -i {videoFilePath} {finalpath}";
            string Command1 =
                " -i d:\\01.avi -vf \"drawtext=fontfile=simhei.ttf: text='南通极客如皋张HC':x=w-tw-10:y=10:fontsize=28:fontcolor=red:shadowy=2\" d:\\output1.avi";
            string Command2 =
                " -i d:\\01.avi -vf \"drawtext=fontfile=simhei.ttf: text='南通极客QQ(827XXXXXX)':y=h-line_h-10:x=(w-mod(30*n\\,w+tw)):fontsize=34:fontcolor=yellow:shadowy=2\" d:\\output2.avi";

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            //非控制台程序必须写完整路径
            p.StartInfo.FileName = AppContext.BaseDirectory + ("/ffmpeg.exe");
            p.StartInfo.Arguments = Command;
            //p.StartInfo.Arguments = Command1;
            //Asp.net 获取当前目录
            p.StartInfo.WorkingDirectory = AppContext.BaseDirectory; //HttpContext.Current.Request.MapPath("~/"); //Environment.CurrentDirectory;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            //开始执行
            p.Start();
            p.BeginErrorReadLine();
            p.WaitForExit();
            p.Close();
            p.Dispose();


            //Console.WriteLine("时间长度：{0}", totalTime);
            //Console.WriteLine("高度：{0}", videoFile.Height);
            //Console.WriteLine("宽度：{0}", videoFile.Width);
            //Console.WriteLine("数据速率：{0}", videoFile.VideoBitRate);
            //Console.WriteLine("数据格式：{0}", videoFile.VideoFormat);
            //Console.WriteLine("比特率：{0}", videoFile.BitRate);
            //Console.WriteLine("文件路径：{0}", videoFile.Path);
            #endregion 自定义逻辑

            Console.ReadKey();
        }
    }
}
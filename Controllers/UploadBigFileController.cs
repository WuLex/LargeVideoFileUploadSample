using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoDemo.Common;
using VideoDemo.Models;

namespace VideoDemo.Controllers
{
    public class UploadBigFileController : Controller
    {
        // GET: UpLoadBigFileController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UpLoadBigFileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UpLoadBigFileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UpLoadBigFileController/UploadVideo
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public int UploadVideo([FromForm] VideoFileModel filemodel, [FromQuery]string opration)
        {
            try
            {
                int slicenum= FileHelper.UploadVideo(filemodel);
                return slicenum;

                //return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                Console.Write(e.ToString());
                return -1;
            }
        }

        // GET: UpLoadBigFileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UpLoadBigFileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

     
    }
}

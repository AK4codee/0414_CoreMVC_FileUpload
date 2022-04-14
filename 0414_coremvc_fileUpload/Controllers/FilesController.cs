using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _0414_coremvc_fileUpload.Controllers
{
    public class FilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                ViewData["Message"] = "Upload file' length is zero, Failed!";
                return View("UploadResult");
            }

            string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadFiles", file.FileName);

            string virtualPath = Url.Content("UploadFiles/" + file.FileName);

            string url = $"{Request.Scheme}://{Request.Host.Value}/{virtualPath}";

            try
            {
                using(var stream = new FileStream(pathFile, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                ViewData["Message"] = "upload file sccess!, filed directory: " +  url ;
            }
            catch (Exception ex)
            {
                ViewData["Message"] = "upload file faled!" + ex.Message;
            }

            return View("UploadResult");
        }

        public async Task<IActionResult> UploadMultiFiles(List<IFormFile> files)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach(IFormFile file in files)
            {
                if (files == null || files.Count() == 0)
                {
                    ViewData["Message"] = "Upload file' length is zero, Failed!";
                   // return View("UploadResult");
                }

                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadFiles", file.FileName);

                string virtualPath = Url.Content("UploadFiles/" + file.FileName);

                string url = $"{Request.Scheme}://{Request.Host.Value}/{virtualPath}";

                try
                {
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    //ViewData["Message"] = "upload file sccess!, filed directory: " + url;
                    map.Add(file.FileName, $"upload file sccess!, filed directory: {url}");
                }
                catch (Exception ex)
                {
                    //ViewData["Message"] = "upload file faled!" + ex.Message;
                    map.Add(file.FileName, $"upload file faled!, filed directory: {ex.Message}");
                }
            }

            ViewData["Message"] = map;
            return View("UploadResults");
        }
    }
}

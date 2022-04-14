using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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
    }
}

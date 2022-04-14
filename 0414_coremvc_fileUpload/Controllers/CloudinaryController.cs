using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace _0414_coremvc_fileUpload.Controllers
{
    public class CloudinaryController : Controller
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryController()
        {
            string cloudname = "ak4";
            string apikey = "467131992575233";
            string apisecret = "8mkNWAplmtnnXq0ahiC7CJNmfyU";

            Account account = new Account(cloudname, apikey, apisecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CloudinaryUploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CloudinaryUploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewData["Message"] = "Upload file' length is zero, Failed!";
                return View("UploadResult");
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

                ViewData["Message"] = "upload file sccess!, filed directory: " + url;
            }
            catch (Exception ex)
            {
                ViewData["Message"] = "upload file faled!" + ex.Message;
            }

            try
            {
                var uploadParam = new ImageUploadParams()
                {
                    File = new FileDescription(pathFile)
                };
                var uploadResult = _cloudinary.Upload(uploadParam);

                ViewData["Message"] = "upload file sccess!, filed directory: " + uploadResult.Url;

                ViewData["FullInfo"] = JsonConvert.SerializeObject(uploadResult.JsonObj);

                int i = 0;
            }
            catch(Exception ex)
            {
                ViewData["Message"] = "upload file faled!" + ex.ToString();
            }

            return View("CloudinaryUploadResult");
        }
    }
}

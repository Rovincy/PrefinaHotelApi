using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace HotelWebApi.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public class FileUpload
        {
            public IFormFile files { get; set; }
        }
        [HttpPost]
        public async Task<string> Post([FromForm] FileUpload objFile)
        {
            if (objFile.files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Uploads\\");
                    }
                    String filePath;
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Uploads\\" + objFile.files.FileName))
                    {
                        //var path = _environment.WebRootPath + "\\Upload\\" + objFile.files.FileName;
                        filePath = String.Format(_environment.WebRootPath + "\\Uploads\\" + objFile.files.FileName);
                        objFile.files.CopyTo(fileStream);
                        fileStream.Flush();
                    }

                    return "\\Uploads\\" + objFile.files.FileName;
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
            else
            {
                return "Failed";
            }
        }

    }

}

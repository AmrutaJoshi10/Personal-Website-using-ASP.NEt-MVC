///////////////////////////////////////////////////////////////
// FilesController.cs - Web Api for handling Files           //
//                                                           //
// Jim Fawcett, CSE686 - Internet Programming, Spring 2019   //
///////////////////////////////////////////////////////////////
/*
 * This package implements Controller for Files Web Api.
 * The web api application:
 * - uploads files to wwwroot/FileStore
 * - displays all files in FileStore
 * - downloads a file from FileStore
 * - [will] delete a file, given its index, from FileStore
 * 
 * Note that Web Api applications don't use action names in their urls.
 * Instead, they use GET, POST, PUT, and DELETE based on the type of
 * the HTTP Request Message.  Also, they don't return views.  They
 * return data.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

using AuthFull.Models;
using Microsoft.Extensions.FileProviders;

namespace AuthFull.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment_;
        private string webRootPath = null;
        private string filePath = null;

        public FilesController(IHostingEnvironment hostingEnvironment)
        {
            hostingEnvironment_ = hostingEnvironment;
            webRootPath = hostingEnvironment_.WebRootPath;
            filePath = Path.Combine(webRootPath, "UploadedFiles");
        }
        
        [Route("[Action]/{filename}")]
        [HttpGet]
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "UploadedFiles", filename);
            if (System.IO.File.Exists(path))
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            else
                return Content("File Not Found");
        }

        [HttpPost("upload")]
        public async Task UploadFilesCon(IFormFile files)
        {
            if (files == null)
                Content("files not selected");

            var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles",
                    files.GetFilename());

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await files.CopyToAsync(stream);
            }
        }
        // GET: api/<controller>
        [Route("[Action]")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> files = null;
            try
            {
                files = Directory.GetFiles(filePath).ToList<string>();
                for (int i = 0; i < files.Count; ++i)
                    files[i] = Path.GetFileName(files[i]);
            }
            catch
            {
                files = new List<string>();
                files.Add("404 - Not Found");
            }
            return files;
        }
        
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
      {
        {".cs", "application/C#" },
        {".txt", "text/plain"},
        {".pdf", "application/pdf"},
        {".doc", "application/vnd.ms-word"},
        {".docx", "application/vnd.ms-word"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},
        {".csv", "text/csv"}
      };
        }
        //----< upload file >--------------------------------------

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Content("files not selected");

            foreach (var file in files)
            {
                var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles",
                        file.GetFilename());

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return RedirectToAction("Files");
        }

        // DELETE api/<controller>/5
        [Route("[Action]/{filename}")]
        [HttpGet]
        public void Delete(string filename)
        {
            if (filename == null)
                Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", "UploadedFiles", filename);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            else
                Content("filename not present");
        }
    }
}

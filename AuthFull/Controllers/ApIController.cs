using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AuthFull.Models;
using Microsoft.Extensions.FileProviders;
using System.Net.Http;
using Newtonsoft.Json;
using AuthFull.Data;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace AuthFull.Controllers
{
    public class ApIController : Controller
    {
        private readonly IFileProvider fileProvider;

        private readonly ApplicationDbContext context_;

        private string url = "https://localhost:5001/api/Files/";

        public ApIController(IFileProvider fileProvider, ApplicationDbContext context)
        {
            this.fileProvider = fileProvider;
            context_ = context;
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Content("files not selected");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {

                    byte[] data;
                    using (var br = new BinaryReader(file.OpenReadStream()))
                        data = br.ReadBytes((int)file.OpenReadStream().Length);

                    ByteArrayContent bytes = new ByteArrayContent(data);
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();
                    string fileName = file.FileName;
                    multiContent.Add(bytes, "files", fileName);
                    response = await client.PostAsync(url + "upload", multiContent);

                }

                if (response.IsSuccessStatusCode)
                {
                    FileDetails fd = new FileDetails();
                    fd.Name = file.FileName;
                    context_.FileStorage.Add(fd);
                    try
                    {
                        context_.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        string err = e.Message;
                        return Content("File addition unsuccessful");
                    }
                }
                else
                    return Content("File upload unsuccessful");
            }
            return RedirectToAction("Result");
        }
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Result()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url + "Get/");
                var files = new List<string>();
                if (resp.IsSuccessStatusCode)
                {
                    var json = await resp.Content.ReadAsStringAsync();
                    JArray jArr = (JArray)JsonConvert.DeserializeObject(json);
                    foreach (var item in jArr)
                        files.Add(item.ToString());
                }
                var fileList = files.Select(x => new FileDetails() { Name = x }).ToList();
                //var fileList = context_.FileStorage;
                return View(fileList);
            }
        }

        public async Task<IActionResult> DeleteFile(int? id, string filename)
        {
            HttpClient client = new HttpClient();

            var result = await client.GetAsync(url + "Delete/" + filename);
            if (result.IsSuccessStatusCode)
            {
                try
                {
                    var file = context_.FileStorage.Where(a => a.Name == filename).FirstOrDefault();
                    var fileSt = context_.FileStorage.Find(file.FileId);
                    if (fileSt != null)
                    {
                        context_.Remove(fileSt);
                        context_.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    // nothing for now
                }
            }
            return RedirectToAction("Result");
        }

        public async Task<IActionResult> Download(string fileName)
        {
            HttpClient client = new HttpClient();

            var result = await client.GetAsync(url + "Download/" + fileName);

            if (result.IsSuccessStatusCode)
            {
                byte[] bytes = await result.Content.ReadAsByteArrayAsync();

                var path = Path.Combine(
                          Directory.GetCurrentDirectory(),
                          "wwwroot", "FileStorage", fileName);

                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            return RedirectToAction("Result");
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
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
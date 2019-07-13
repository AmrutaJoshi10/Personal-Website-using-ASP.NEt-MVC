///////////////////////////////////////////////////////////////
// ConsoleClient.cs - Client for WebApi FilesController      //
//                                                           //
// Jim Fawcett, CSE686 - Internet Programming, Spring 2019   //
///////////////////////////////////////////////////////////////
/*
 * - Based on Asp.Net Core Framework, this client project generates
 *   dynamic link library that can be hosted by Visual Studio or
 *   dotnet CLI.
 * - It provides options via its command line, e.g.:
 *   - url /fl            displays list of files in server's FileStore
 *   - url /up fileSpec   uploades fileSpec to FileStore
 *   - url /dn n          downloads nth file in FileStore
 *   - url /dl n          deletes nth file in FileStore
 */
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;  // must install Newtonsoft package from Nuget
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ConsoleClient
{
    class CoreConsoleClient
    {
        public HttpClient client { get; set; }

        private string baseUrl_;

        CoreConsoleClient(string url)
        {
            baseUrl_ = url;
            client = new HttpClient();
        }
        //----< upload file >--------------------------------------

        public async Task<HttpResponseMessage> SendFile(string fileSpec)
        {
            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            byte[] data = File.ReadAllBytes(fileSpec);
            ByteArrayContent bytes = new ByteArrayContent(data);
            string fileName = Path.GetFileName(fileSpec);
            multiContent.Add(bytes, "files", fileName);

            return await client.PostAsync(baseUrl_, multiContent);
        }
        //----< get list of files in server FileStorage >----------

        public async Task<IEnumerable<string>> GetFileList()
        {
            HttpResponseMessage resp = await client.GetAsync(baseUrl_+"/"+"Get/");
            var files = new List<string>();
            if (resp.IsSuccessStatusCode)
            {
                var json = await resp.Content.ReadAsStringAsync();
                JArray jArr = (JArray)JsonConvert.DeserializeObject(json);
                foreach (var item in jArr)
                    files.Add(item.ToString());
            }
            return files;
        }
        //----< download the id-th file >--------------------------

        public async Task<HttpResponseMessage> GetFile(string filename)
        {
            var result = await client.GetAsync(baseUrl_ + "/" + "Download/" + filename);
            Console.Write(result);
            if (result.IsSuccessStatusCode)
            {
                byte[] bytes = await result.Content.ReadAsByteArrayAsync();

                var path = Path.GetFullPath("../../../../AuthFull/wwwroot/FileStorage");
                try
                {
                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                    Console.Write("File Downloaded Successfully");
                }
                catch(Exception e)
                {

                }
            }
                return result;
        }
        //----< delete the id-th file from FileStorage >-----------

        public async Task<HttpResponseMessage> DeleteFile(string filename)
        {
            return await client.GetAsync(baseUrl_ + "/" + "Delete/" + filename);
        }
        //----< usage message shown if command line invalid >------

        static void showUsage()
        {
            Console.Write("\n  Command line syntax error: expected usage:\n");
            Console.Write("\n    http[s]://machine:port /option [filespec]\n\n");
        }
        //----< validate the command line >------------------------

        static bool parseCommandLine(string[] args)
        {
            if (args.Length < 2)
            {
                showUsage();
                return false;
            }
            if (args[0].Substring(0, 4) != "http")
            {
                showUsage();
                return false;
            }
            if (args[1][0] != '/')
            {
                showUsage();
                return false;
            }
            return true;
        }
        //----< display command line arguments >-------------------

        static void showCommandLine(string[] args)
        {
            string arg0 = args[0];
            string arg1 = args[1];
            string arg2;
            if (args.Length == 3)
                arg2 = args[2];
            else
                arg2 = "";
            Console.Write("\n  CommandLine: {0} {1} {2}", arg0, arg1, arg2);
        }

        static void Main(string[] args)
        {
            Console.Write("\n  CoreConsoleClient");
            Console.Write("\n ===================\n");

            if (!parseCommandLine(args))
            {
                return;
            }
            Console.Write("Press key to start: ");
            Console.ReadKey();

            string url = args[0];
            CoreConsoleClient client = new CoreConsoleClient(url);

            showCommandLine(args);
            Console.Write("\n  sending request to {0}\n", url);

            switch (args[1])
            {
                case "/fl":
                    Task<IEnumerable<string>> tfl = client.GetFileList();
                    var resultfl = tfl.Result;
                    foreach (var item in resultfl)
                    {
                        Console.Write("\n  {0}", item);
                    }
                    break;
                case "/up":
                    Task<HttpResponseMessage> tup = client.SendFile(args[2]);
                    Console.Write(tup.Result);
                    break;
                case "/dn":
                    string fileName = args[2];
                    List<string> fileListdl = new List<string>();
                    Task<IEnumerable<string>> tfldn = client.GetFileList();
                    var resultfldn = tfldn.Result;
                    foreach (var item in resultfldn)
                    {
                        fileListdl.Add(item);
                    }
                    if (fileListdl.Contains(fileName))
                    {
                        Task<HttpResponseMessage> tdn = client.GetFile(fileName);
                    }
                    else
                        Console.Write("File not found");
                    break;
                case "/dl":
                    string file = args[2];
                    List<string> fileList = new List<string>();
                    Task<IEnumerable<string>> tfldl = client.GetFileList();
                    var resultfldl = tfldl.Result;
                    foreach (var item in resultfldl)
                    {
                        fileList.Add(item);
                    }
                    if (fileList.Contains(file))
                    {
                        Task<HttpResponseMessage> tdl = client.DeleteFile(file);
                        Console.Write(tdl.Result);
                        if (tdl.Result.IsSuccessStatusCode)
                        {
                            Console.Write("File Deleted Successfully");
                        }
                    }
                    else
                        Console.Write("File not found");
                    break;
            }

            Console.WriteLine("\n  Press Key to exit: ");
            Console.ReadKey();
        }
    }
}

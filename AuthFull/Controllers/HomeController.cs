using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using AuthFull.Models;
using AuthFull.Data;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AuthFull.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context_;
        private readonly UserManager<IdentityUser> _userManager;

        private const string sessionId_ = "SessionId";

        private readonly IHostingEnvironment he;
        public HomeController(IHostingEnvironment e, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            he = e;
            context_ = context;
            _userManager = userManager;
        }
        //----< show Applicant >-------------------------------

        public IActionResult Index()
        {
            return View(context_.Applicant.ToList<Applican>());
        }

        public IActionResult ShowFields(string fullName, IFormFile pic)
        {
            ViewData["fname"] = fullName;
            if (pic != null)
            {
                var fileName = Path.Combine(he.WebRootPath, Path.GetFileName(pic.FileName));
                pic.CopyTo(new FileStream(fileName, FileMode.Create));
                ViewData["fileLocation"] = "/" + Path.GetFileName(pic.FileName);
            }
            return View();
        }

        public IActionResult DeleteFields()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LINKEDIN.jpg");

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult ReviewApplication()
        {
            ViewModel myModel = new ViewModel();
            myModel.Applicann = context_.Applicant.ToList<Applican>();
            myModel.Academii = context_.Academics.ToList<Academic>();
            myModel.WorkOp = context_.WorkEx.ToList<WorkExperience>();
            return View(myModel);
        }

        public IActionResult ExternalLink(string name)
        {
            return Redirect(name);
        }

        //----< show list of Academic Information, ordered by School_Of_Studying >------------

        public IActionResult AcademicInformation()
        {
            var acts = context_.Academics.Include(a => a.applicant);
            var orderedacts = acts.OrderBy(a => a.School_Of_Studying)
              .OrderBy(a => a.applicant)
              .Select(a => a);
            return View(orderedacts);
        }

        //----< displays form for creating a applicant Information >----------------
      

        [HttpGet]
        
        public IActionResult CreateApplicant(int id)
        {
            var model = new Applican();
            return View(model);
        }

        [HttpPost]
        //----< posts back new applicant Information >---------------------

        [HttpPost]
        

        public IActionResult CreateApplicant(int id, Applican apt)
        {
            context_.Applicant.Add(apt);
            context_.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ProfileDetails(int id)
        {
            var model = new Applican();
            return View(model);
        }

        [HttpPost]
        //----< posts back new applicant Information >---------------------

        public IActionResult ProfileDetails(int id, Applican apt)
        {
            var app = context_.Applicant;
            if(app.Count() == 0) {
                if(apt != null)
                    context_.Applicant.Add(apt);
                try
                {
                    context_.SaveChanges();
                }
                catch (Exception) {

                }
            }
            else
            {
                return Content("No more Applicants can be added");
            }
            
            return RedirectToAction("Index");
        }



        // [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]

        public IActionResult DeleteApplicants(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var app = context_.Applicant.Find(id);
                if (app != null)
                {
                    context_.Remove(app);
                    context_.SaveChanges();
                }
                if (id == 1)
                {
                    context_.Remove(app);
                    context_.SaveChanges();

                }


            }
            catch (Exception)
            {
                // nothing for now
            }
            return RedirectToAction("Index");
        }


        //---<Create Work Experience Details>---------------
        [HttpGet]
        public IActionResult WorkExperienceDetails(int id)
        {
            var model = new WorkExperience();
            return View(model);
        }

        [HttpPost]
        //----< posts back new applicant Information >---------------------

        public IActionResult WorkExperienceDetails(int id, WorkExperience wet)
        {
            context_.WorkEx.Add(wet);
            context_.SaveChanges();
            return RedirectToAction("ReviewApplication");
        }

        public IActionResult WorkInformation()
        {
            var wcts = context_.WorkEx.Include(a => a.applicantN);
            var orderedwcts = wcts.OrderBy(a => a.No_Of_Years)
              .OrderBy(a => a.applicantN)
              .Select(a => a);
            return View(orderedwcts);
        }



        //----< add new Work Information for the Applicant >--------------------------

        [HttpGet]
        public IActionResult AddWorkDetails(int id)
        {
            HttpContext.Session.SetInt32(sessionId_, id);

            // this works too
            // TempData[sessionId_] = id;

            Applican app = context_.Applicant.Find(id);
            if (app == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            WorkExperience act = new WorkExperience();
            return View(act);
        }

        //----< Add new AcademicInformation to Applicant >--------------------------

        [HttpPost]
        public IActionResult AddWorkDetails(int? id, WorkExperience wca)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            // retreive the target course from static field

            int? ApplicantId_ = HttpContext.Session.GetInt32(sessionId_);

            // this works too
            // int courseId_ = (int)TempData[sessionId_];

            var appli = context_.Applicant.Find(ApplicantId_);

            if (appli != null)
            {
                if (appli.academs == null)  // doesn't have any lectures yet
                {
                    List<WorkExperience> acade = new List<WorkExperience>();
                    appli.work = acade;
                }
                appli.work.Add(wca);

                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // do nothing for now
                }
            }
            return RedirectToAction("Index");
        }





        //----< gets form to edit a specific Academic Information via id >---------
        [HttpGet]
        public IActionResult EditWorkDetails(int? id)
        {
            if (id == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            WorkExperience acc = context_.WorkEx.Find(id);

            if (acc == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(acc);
        }

        //----< posts back edited results for specific AcademicInformation >------

        [HttpPost]
        public IActionResult EditWorkDetails(int? id, WorkExperience lct)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var adp = context_.WorkEx.Find(id);

            if (adp != null)
            {
                adp.Company = lct.Company;
                adp.JobTitle = lct.JobTitle;
                adp.No_Of_Years = lct.No_Of_Years;
                adp.Responsibilites = lct.Responsibilites;
                adp.Technologies_Used = lct.Technologies_Used;
                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // do nothing for now
                }
            }
            return RedirectToAction("ReviewApplication");
        }





        //----< shows details for Applicant >----------------------

        public ActionResult ApplicantDetails(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Applican app = context_.Applicant.Find(id);

            if (app == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var acads = context_.Academics.Where(a => a.applicant == app);

            app.academs = acads.OrderBy(a => a.School_Of_Studying).Select(a => a).ToList<Academic>();
            if (app.academs == null)
            {
                app.academs = new List<Academic>();
                Academic acd = new Academic();
                acd.Degree = "none";
                acd.School_Of_Studying = "none";
                acd.Major = "none";
                acd.Graduation_Date = "none";
                app.academs.Add(acd);
            }
            return View(app);
        }

        // get form to edit applicant details.

        [HttpGet]
        public IActionResult EditAppDetails(int? id)
        {
            if (id == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Applican applicants = context_.Applicant.Find(id);
            if (applicants == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(applicants);
        }

        //----< posts back edited results for the Applicant >------
        [HttpPost]
        public IActionResult EditAppDetails(int? id, Applican apt)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var appli = context_.Applicant.Find(id);
            if (appli != null)
            {
                appli.FirstName = apt.FirstName;
                appli.LastName = apt.LastName;
                appli.EmailId = apt.EmailId;
                appli.Address = apt.Address;
                appli.City = apt.City;
                appli.Country = apt.Country;
                appli.PinCode = apt.PinCode;
                appli.Date_Of_Birth = apt.Date_Of_Birth;
                appli.Gender = apt.Gender;
                appli.Phone_Number = apt.Phone_Number;



                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // do nothing for now
                }
            }
            return RedirectToAction("ReviewApplication");
        }

        //----< shows form for creating Academic Information >------------------

        [HttpGet]
        public IActionResult CreateAcademic(int id)
        {
            var model = new Academic();
            return View(model);
        }

        //----< posts back new  Academic Information >---------------------

        [HttpPost]
        public IActionResult CreateAcademic(int id, Academic act)
        {
            context_.Academics.Add(act);
            context_.SaveChanges();
            return RedirectToAction("AcademicInformation");
        }



        //----< add new Academic Information for the Applicant >--------------------------


        [HttpGet]
        public IActionResult AddAcademic(int id)
        {
            var appli = context_.Applicant;
            if(appli.Count() == 1)
            {
                foreach(var a in appli)
                {
                    id = a.ApplicantId;
                }
            }
            else
            {
                return Content("Applicant Not Found");
            }
            HttpContext.Session.SetInt32(sessionId_, id);

            // this works too
            // TempData[sessionId_] = id;

            Applican app = context_.Applicant.Find(id);
            if (app == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Academic act = new Academic();
            return View(act);
        }

        //----< Add new AcademicInformation to Applicant >--------------------------

        [HttpPost]
        public IActionResult AddAcademic(int? id, Academic aca)
        {
            var applica = context_.Applicant;
            if (applica.Count() == 1)
            {
                foreach (var a in applica)
                {
                    id = a.ApplicantId;
                }
            }
            else
            {
                return Content("Applicant Not Found");
            }
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            // retreive the target course from static field

            int? ApplicantId_ = HttpContext.Session.GetInt32(sessionId_);

            // this works too
            // int courseId_ = (int)TempData[sessionId_];

            var appli = context_.Applicant.Find(ApplicantId_);

            if (appli != null)
            {
                if (appli.academs == null)  // doesn't have any lectures yet
                {
                    List<Academic> acade = new List<Academic>();
                    appli.academs = acade;
                }
                appli.academs.Add(aca);

                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // do nothing for now
                }
            }
            return RedirectToAction("ReviewApplication");
        }




        //----< gets form to edit a specific Academic Information via id >---------
        [HttpGet]
        public IActionResult EditAcademic(int? id)
        {
            if (id == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Academic acc = context_.Academics.Find(id);

            if (acc == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(acc);
        }

        //----< posts back edited results for specific AcademicInformation >------

        [HttpPost]
        public IActionResult EditAcademic(int? id, Academic lct)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var adp = context_.Academics.Find(id);

            if (adp != null)
            {
                adp.School_Of_Studying = lct.School_Of_Studying; 
                adp.Degree = lct.Degree;
                adp.Major = lct.Major;

                adp.Graduation_Date = lct.Graduation_Date;
                adp.GPA = lct.GPA;
                adp.project_Links = lct.project_Links;
                adp.Description_About_Projects = lct.Description_About_Projects;

                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // do nothing for now
                }
            }
            return RedirectToAction("ReviewApplication");
        }

        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]

        public IActionResult DeleteAcademic(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var academi = context_.Academics.Find(id);
                if (academi != null)
                {
                    context_.Remove(academi);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {
                // nothing for now
            }
            return RedirectToAction("ReviewApplication");
        }


        public IActionResult DeleteWorkExp(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var workexpe = context_.WorkEx.Find(id);
                if (workexpe != null)
                {
                    context_.Remove(workexpe);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {
                // nothing for now
            }
            return RedirectToAction("ReviewApplication");
        }

        public IActionResult AddComment(int id)
        {
            var model = new Comment();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int id, Comment comm)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            comm.UserName = user.UserName;
            context_.CommentsSection.Add(comm);
            context_.SaveChanges();
            return RedirectToAction("Comment");
        }
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Comment()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var admin = await _userManager.GetUsersInRoleAsync("Admin");
            var adminUserName = admin[0].UserName;
            if (adminUserName == user.UserName)
            {
                var commentList = context_.CommentsSection; 
                return View(commentList);
            }
            else
            {
                var commentList = context_.CommentsSection.Where(a => a.UserName == user.UserName);
                return View(commentList);
            }
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

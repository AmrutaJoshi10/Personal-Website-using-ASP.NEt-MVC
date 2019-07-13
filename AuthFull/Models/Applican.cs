using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthFull.Models
{
    public class Applican
    {
        [Key]
        public int ApplicantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Date_Of_Birth { get; set; }
        public string Gender { get; set; }
        public string Phone_Number { get; set; }
        public ICollection<Academic> academs { get; set; }
        public ICollection<WorkExperience> work { get; set; }
    }

    public class Academic
    {

        public int AcademicId { get; set; }
        public string Degree { get; set; }
        public string School_Of_Studying { get; set; }
        public string Major { get; set; }
        public string Graduation_Date { get; set; }
        public string GPA { get; set; }
        public string project_Links { get; set; }
        [MaxLength(5000)]
        public string Description_About_Projects { get; set; }
        public int? ApplicantId { get; set; }
        public Applican applicant { get; set; }

    }
    public class WorkExperience
    {
        [Key]
        public int WorkId { get; set; }
        [MaxLength(5000)]
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string No_Of_Years { get; set; }
        public string Responsibilites { get; set; }
        public string Technologies_Used { get; set; }
        public int? ApplicantIds { get; set; }
        public Applican applicantN { get; set; }

    }




}

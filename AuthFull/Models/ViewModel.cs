using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthFull.Models
{
    public class ViewModel
{
       
        public IEnumerable<Applican> Applicann { get; set; }
        public IEnumerable<Academic> Academii { get; set; }
        public IEnumerable<WorkExperience> WorkOp { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthFull.Models
{

    public class FileDetails
    {
        [Key]
        public int FileId { get; set; }
        public string Name { get; set; }
    }
}

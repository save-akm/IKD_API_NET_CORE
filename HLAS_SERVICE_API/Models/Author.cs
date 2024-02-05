using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HLAS_SERVICE_API.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LasttName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }
    }
}
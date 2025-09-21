using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EntityLayer.Concrete
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string User { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}

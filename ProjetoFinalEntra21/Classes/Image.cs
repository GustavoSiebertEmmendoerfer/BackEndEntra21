using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models.Classes_Models
{
    public class Image
    {
        [Key]           
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImageCaption { get; set; }
        public User Restaurant { get; set; }
    }
}

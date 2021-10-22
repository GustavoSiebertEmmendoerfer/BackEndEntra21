using ProjetoFinalEntra21.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models
{
    public class Plate
    {
        [Key]
        public int PlateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public virtual  User Restaurant { get; set; }
        public string PhotoURL { get; set; }

    }
}

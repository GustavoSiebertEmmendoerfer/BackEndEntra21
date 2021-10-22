using ProjetoFinalEntra21.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string Status { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime OrderEnd { get; set; }
        public virtual User Client { get; set; }
        public string RestaurantEmail { get; set; }
    }
}

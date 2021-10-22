using ProjetoFinalEntra21.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models
{
    public class OrderBindingModel
    {
        public string Status { get; set; }
        public string ClientEmail { get; set; }
        public string RestaurantEmail { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}

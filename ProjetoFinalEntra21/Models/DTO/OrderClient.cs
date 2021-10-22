using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models.DTO
{
    public class OrderClient
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string RestaurantName { get; set; }
        public string plateName { get; set; }
        public double price{ get; set; }
        public int Quantity { get; set; }
    }
}

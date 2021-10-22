using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models.BindingModels
{
    public class OrderItemBindingModel
    {
        public int Quantity { get; set; }
        public int OrderID { get; set; }
        public int PlateID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models.BindingModels
{
    public class UpdateUserBindingModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoURL { get; set; }
        public string Description { get; set; }
    }
}

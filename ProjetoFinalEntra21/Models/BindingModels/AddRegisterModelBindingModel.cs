using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models.BindingModels
{
    public class AddRegisterModelBindingModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string CPF { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public List<string> Roles { get; set; }

    }
}

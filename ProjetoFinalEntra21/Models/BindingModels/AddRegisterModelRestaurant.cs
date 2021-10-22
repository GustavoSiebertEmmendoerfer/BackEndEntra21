using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models.BindingModels
{
    public class AddRegisterModelRestaurant
    {
        public string Username { get; set; }

        public string OpenTime { get; set; }

        public string CloseTime { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string CNPJ { get; set; }

        public string Password { get; set; }

        public string PhotoURL { get; set; }

        public List<string> Roles { get; set; }
    }
}

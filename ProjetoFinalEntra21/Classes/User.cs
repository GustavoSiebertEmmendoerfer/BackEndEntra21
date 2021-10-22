    using Microsoft.AspNetCore.Identity;
using ProjetoFinalEntra21.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models
{
    public class User: IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string CPF { get; set; }
        public string DateCreated { get; set; }
        public string DateModified{ get; set; }
        public string Cnpj { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public string PhotoURL { get; set; }
        public string Description { get; set; }
    }
}

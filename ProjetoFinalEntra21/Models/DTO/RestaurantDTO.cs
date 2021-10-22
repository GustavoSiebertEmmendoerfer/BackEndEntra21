using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Data
{
    public class RestaurantDTO
    {
        public RestaurantDTO(string fullName, string email, string userName, string address, string description, string id, string photoURL)
        {
            FullName = fullName;
            Email = email;
            UserName = userName;
            Address = address;
            Description = description;
            Id = id;
            PhotoURL = photoURL;
        }
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string PhotoURL { get; set; }
    }
}

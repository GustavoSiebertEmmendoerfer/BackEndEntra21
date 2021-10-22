using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Data
{
    public class UserDTO
    {
        public UserDTO(string fullName, string email, string userName, string dateCreated, List<string> roles,string address, string photoURL,string userId)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
            UserName = userName;
            DateCreated = dateCreated;
            Roles = roles;
            Address = address;
            PhotoURL = photoURL;
        }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string PhotoURL { get; set; }
        public string DateCreated { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Data
{
    public class PlatesDTO
    {
        public PlatesDTO(int id,string name, double price,string description, string photoURL)
        {
            PlateId = id;
            Name = name;
            Price = price;
            Description = description;
            PhotoURL = photoURL;
        }
        public int PlateId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string PhotoURL { get; set; }
    }
}

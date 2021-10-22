using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinalEntra21.Models.BindingModels
{
    public class ImageModel
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImageCaption { get; set; }
        public string RestaurantEmail { get; set; }
    }
}

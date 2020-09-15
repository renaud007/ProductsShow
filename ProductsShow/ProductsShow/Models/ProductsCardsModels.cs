using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsShow.Models
{
    public class ProductsCardsModels
    {
        public string Name { get; set; }
        public string Informations { get; set; }
        public string ProductColor { get; set; }
        public string Image { get; set; }
        public List<string> Posters { get; set; }
    }
}

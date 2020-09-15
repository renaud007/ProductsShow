using ProductsShow.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProductsShow.ViewModels
{
    public   class ProductsCardsVM
    {
        public ObservableCollection<ProductsCardsModels> Products { get; set; }
        public float MyProperty = 25;
        
        public ProductsCardsVM()
        {
            Products = new ObservableCollection<ProductsCardsModels>()
            {
               new ProductsCardsModels
               {  

                   Name="Pegasus 20000DH",  
                   ProductColor="#38C0FF",
                   Image="aa",
                   Informations="Ce texte est un exemple et ocupe la place des differentes informations concernant le pc ci-dessus. Bien entendu ,c'est à titre educatif...Ce texte est un exemple et ocupe la place des differentes informations concernant le pc ci-dessus. Bien entendu ,c'est à titre educatif",

                   Posters=new List<string>()
                   {
                       "pcione.jpg","pcitwo.jpg","pcione.jpg","pcitwo.jpg"
                   } 
               },
                
                new ProductsCardsModels
               {
                   Name="Breaker 15000DH",

                   Informations="Ce texte est un exemple et ocupe la place des differentes informations concernant le pc ci-dessus. Bien entendu ,c'est à titre educatif...Ce texte est un exemple et ocupe la place des differentes informations concernant le pc ci-dessus. Bien entendu ,c'est à titre educatif",

                   ProductColor="#FF0605",
                   Image="b" 
                   ,
                   Posters=new List<string>()
                   {
                       "pcione.jpg","pcitwo.jpg","pcione.jpg","pcitwo.jpg"
                   }
               },new ProductsCardsModels
               {
                   Name="Spiking 17000DH",

                   Informations="Ce texte est un exemple et ocupe la place des differentes informations concernant le pc ci-dessus. Bien entendu ,c'est à titre educatif...Ce texte est un exemple et ocupe la place des differentes informations concernant le pc ci-dessus. Bien entendu ,c'est à titre educatif",

                   ProductColor="#014DBB",
                   Image="c",
                   Posters=new List<string>()
                   {
                       "pcione.jpg","pcitwo.jpg","pcione.jpg","pcitwo.jpg"
                   }
               },new ProductsCardsModels
               {
                   Name="Gribo 12000DHBM",  
                   Informations="Ce texte est un exemple et ocupe la place des differentes informations concernant le pc ci-dessus. Bien entendu ,c'est à titre educatif...Ce texte est un exemple et ocupe la place des differentes informations concernant le pc ci-dessus. Bien entendu ,c'est à titre educatif",

                   ProductColor="#E32423",
                   Image="dd",
                   Posters=new List<string>()
                   {
                       "pcione.jpg","pcitwo.jpg","pcione.jpg","pcitwo.jpg"
                   }
               } 
            };
            

        }
    }
}

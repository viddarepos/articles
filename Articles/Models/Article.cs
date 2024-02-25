using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Articles.Models
{
    public class Article
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Rating { get; set; }

        public Article()
        {
            ID = -1;
            Name = "Noname";
            Category = "Nocategory";
            Price = 0;
            Rating = 0;
        }

        public Article(int id, string name, string category, decimal price)
        {
            ID = id;
            Name = name;
            Category = category;
            Price = price;
            Rating = 0;
        }

    }
}

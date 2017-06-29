using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Stocks
    {
        Stocks()
        {
            this.fuelEconomy = -1;
        }
        [Required]
        public decimal price{get; set;}
        [Required]
        public int year { get; set; }
        [Required]
        public int kilometer { get; set; }
        [Required]
        public int fuelType { get; set; }
        [Required]
        public int city { get; set; }
        [Required]
        public int color { get; set; }
        public double fuelEconomy { get; set; }
        [Required]
        public int make { get; set; }
        [Required]
        public int model { get; set; }
        [Required]
        public int version { get; set; }

    }
    public class Db
    {
        public int Id{ get; set; }
    }
    public class Check
    {
        public int exist { get; set; }
    }
    public class Entity
    {
        public  int color { get; set; }
        public int model{ get; set; }
        public  int fuel { get; set; }
        public int city { get; set; }
        public  int make { get; set; }
        public  int version { get; set; } 
    }
    
}
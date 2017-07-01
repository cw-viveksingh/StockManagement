using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockImageProducer.Models
{
    public class Url
    {
        [Required]
        public string urlName { get; set; }
    }
}
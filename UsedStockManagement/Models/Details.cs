using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsedStockManagement.Models
{
    [Serializable]
    public class Details
    {

        public decimal price { get; set; }
        public int yer { get; set; }
        public int kilometer { get; set; }
        public int fuelType { get; set; }
        public int city { get; set; }
        public int color { get; set; }
        public double fuelEconomy { get; set; }
        public int make { get; set; }
        public int model { get; set; }
        public int version { get; set; }

    }
}
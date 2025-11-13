using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BanDoGiaDung.Models
{
    public class ProductCompareViewModel
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public double price { get; set; }
        public double Discount_price { get; set; }
        public string image { get; set; }
        public string specifications { get; set; }
        public string brand_name { get; set; }
        public string genre_name { get; set; }
        public string quantity { get; set; }
    }
}
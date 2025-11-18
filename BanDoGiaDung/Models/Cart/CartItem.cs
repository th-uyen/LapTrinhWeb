using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Cần cho [Key]
using System.Linq;
using System.Web;

namespace BanDoGiaDung.Models.Cart // <-- Bà check lại namespace này xem đúng chưa
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public int Stock { get; set; }

        // SỬA LẠI TÊN Ở ĐÂY (từ "CartItem" thành "TotalPrice")
        public double TotalPrice
        {
            get { return Quantity * Price; }
        }
    }
}
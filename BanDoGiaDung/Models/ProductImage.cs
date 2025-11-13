using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity.Spatial;
using BanDoGiaDung.Models;


namespace BanDoGiaDung.Models
{
    [Table("ProductImages")]
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int product_img_id { get; set; }

       // KHÓA NGOẠI LÀ product_id
        public int product_id { get; set; }

        // public int genre_id { get; set; }
        // public int disscount_id { get; set; }

        [StringLength(500)]
        public string image { get; set; }

        // Navigation property (Đã đúng)
        public virtual Product Product { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BanDoGiaDung.Models;

namespace BanDoGiaDung.Models
{
    [Table("Genre")]
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int genre_id { get; set; }

        [Required]
        [StringLength(50)]
        public string genre_name { get; set; }

        public DateTime create_at { get; set; }

        [Required]
        [StringLength(100)]
        public string create_by { get; set; }

        [Required]
        [StringLength(100)]
        public string update_by { get; set; }

        public DateTime update_at { get; set; }

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; }

        public Genre()
        {
            Products = new HashSet<Product>();
        }
    }
}
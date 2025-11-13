using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Data.Entity.Spatial;
using BanDoGiaDung.Models;

namespace BanDoGiaDung.Models
{
    [Table("Product")]
    public class Product
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Feedbacks = new HashSet<Feedback>();
            Oder_Detail = new HashSet<Oder_Detail>();
            ProductImages = new HashSet<ProductImage>();
        }

        // 1. KHÓA CHÍNH DUY NHẤT CỦA SẢN PHẨM
        [Key] // Chỉ cần [Key] là đủ
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int product_id { get; set; }


        // 2. CÁC KHÓA NGOẠI (FOREIGN KEYS)

        [Required(ErrorMessage = "Vui lòng chọn thể loại")]
        // BỎ [Key] và [Column(Order = 1)] ở đây
        public int genre_id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int brand_id { get; set; }

        // BỎ [Key] và [Column(Order = 2)] ở đây
        // Sửa thành int? (nullable) vì sản phẩm có thể không có giảm giá
        public int? disscount_id { get; set; }


        // --- CÁC THUỘC TÍNH KHÁC CỦA SẢN PHẨM ---
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được quá 200 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string product_name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        public double price { get; set; }

        public long view { get; set; }
        public long buyturn { get; set; }

        // ... (code cho 'quantity' của bạn) ...
        private string _quantity;
        [StringLength(10)]
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public string quantity
        {
            get { return ((this._quantity != "" && this._quantity != null) ? this._quantity.Trim() : this._quantity); }
            set { this._quantity = (value == null) ? "" : value.Trim(); }
        }

        [StringLength(1)] public string status { get; set; }
        [Required][StringLength(100)] public string create_by { get; set; }
        public DateTime create_at { get; set; }
        [StringLength(100)]
        public string update_by { get; set; }
        public DateTime update_at { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập loại sản phẩm")]
        public int? type { get; set; }
        public string specifications { get; set; }
        public string image { get; set; }
        public string description { get; set; }


        // --- CÁC NAVIGATION PROPERTIES (Đã đúng) ---
        public virtual Brand Brand { get; set; }
        public virtual Discount Discount { get; set; }
        public virtual Genre Genre { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Oder_Detail> Oder_Detail { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }


        // --- CÁC THUỘC TÍNH [NotMapped] (Đã đúng) ---
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
        [NotMapped]
        public HttpPostedFileBase[] ImageUploadMulti { get; set; }
    }
}
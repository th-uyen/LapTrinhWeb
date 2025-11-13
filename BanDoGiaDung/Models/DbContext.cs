using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BanDoGiaDung.Models
{
    public class GiaDungDbContext : DbContext
    {
        public GiaDungDbContext() : base("name=MyConnectionString")
        {

        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<AccountAddress> AccountAddresses { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Districts> Districts { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Oder_Detail> Oder_Details { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderAddress> OrderAddresses { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Provinces> Provinces { get; set; }
        public virtual DbSet<ReplyFeedback> ReplyFeedbacks { get; set; }
        public virtual DbSet<Wards> Wards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Districts>()
                .HasRequired(d => d.Provinces)
                .WithMany(p => p.Districts)
                .HasForeignKey(d => d.province_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wards>()
                .HasRequired(w => w.Districts)
                .WithMany(d => d.Wards)
                .HasForeignKey(w => w.district_id)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Delivery>()
                .Property(p => p.price)
                .HasPrecision(19, 4);


            // Tắt cascade delete cho quan hệ Feedback -> ReplyFeedback
            modelBuilder.Entity<ReplyFeedback>()
                .HasRequired(rf => rf.Feedback)
                .WithMany(f => f.ReplyFeedbacks)
                .HasForeignKey(rf => rf.feedback_id)
                .WillCascadeOnDelete(false);
        }
    }
}
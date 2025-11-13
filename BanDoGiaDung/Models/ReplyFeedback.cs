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
    [Table("ReplyFeedback")]
    public class ReplyFeedback
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int rep_feedback_id { get; set; }

        public int feedback_id { get; set; }

        public int account_id { get; set; }

        public string content { get; set; }

        [StringLength(1)]
        public string stastus { get; set; }
        public DateTime create_at { get; set; }

        public virtual Accounts Account { get; set; }

        public virtual Feedback Feedback { get; set; }
    }
}
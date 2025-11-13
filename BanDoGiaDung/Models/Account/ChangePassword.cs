using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanDoGiaDung.Models.Account
{
    public class ChangePassword
    {

        public string UserName { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu cũ", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu mới", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác thực không trùng nhau")]
        public string ConfirmPassword { get; set; }
    }
}
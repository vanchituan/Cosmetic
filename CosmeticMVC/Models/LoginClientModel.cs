using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CosmeticMVC.Models
{
    public class LoginClientModel
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "User_ErrorID")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "User")]
        [Display(Name = "Mật khẩu")]
        public string Password { set; get; }
    }
}
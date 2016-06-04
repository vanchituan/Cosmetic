using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CosmeticMVC.Models
{
    public class LoginModel
    {
        //[Display(Name = "User_ErrorID", ResourceType =typeof(StaticResources.Resources))]
        [Required(ErrorMessage = "Mời bạn nhập Username")]
        [Display(Name ="Tài khoản")]
        public string UserName { get; set; }


        //[Display(Name = "User_ErrorPass", ResourceType = typeof(StaticResources.Resources))]
        [Required(ErrorMessage = "Mời bạn nhập Password")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }


    }
}
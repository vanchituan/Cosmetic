using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticMVC.Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }

        /// <summary>
        /// true thì đi tiếp vào controller ! false thi xử lý ở unauthorized
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (UserLogin)HttpContext.Current.Session[CommonConstant.UserSession];
            if (session == null)
            {
                return false;
            }
            //dãy quyền mà user có duoc trong hệ thống
            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(session.Username); // Call another method to get rights of the user from DB

            if (privilegeLevels.Contains(this.RoleID) || session.GroupID == CommonConstants.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;// không có quyền truy cập
            }
        }

        /// <summary>
        /// Hàm xử lý khi AuthorizeCore = false
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/_401.cshtml"
            };
        }

        /// <summary>
        /// trả về những quyền mà username truyền vào, có được trong hệ thống (CREDENTIAL ID)
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private List<string> GetCredentialByLoggedInUser(string userName)
        {
            var credentials = (List<string>)HttpContext.Current.Session[CommonConstant.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}
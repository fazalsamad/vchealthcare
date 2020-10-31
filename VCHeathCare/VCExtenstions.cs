using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using VCHeathCare.Models;

namespace VCHeathCare
{
    public static class VCExtenstions
    {
        public static string Name(this IIdentity user)
        {
            var ctx = new ApplicationDbContext();
            var _user = ctx.Users.Where(u => u.UserName == user.Name).FirstOrDefault();
            if(_user == null)
            {
                return string.Empty;
            }
            else
            {
                return _user.Name;
            }
        }

        public static ApplicationUser User(this IIdentity user)
        {
            var ctx = new ApplicationDbContext();

            var currentUser = ctx.Users.Where(u => u.UserName == user.Name).FirstOrDefault();

            return currentUser;
        }
    }
}
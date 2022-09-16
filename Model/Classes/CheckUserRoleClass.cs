using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowers.Model.Classes
{
    internal class CheckUserRoleClass
    {
        public static int CheckUserRole(string Login, string Password) =>
         CheckLoginClass.CheckLogin(Login, Password) ? TradeEntities.GetContext().User.First(b => b.UserLogin == Login && b.UserPassword == Password).UserRole : 0;

    }
}

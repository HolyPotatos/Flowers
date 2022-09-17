using System.Linq;

namespace Flowers.Model.Classes
{
    internal class CheckUserRoleClass
    {
        public static int CheckUserRole(string Login, string Password) =>
         CheckLoginClass.CheckLogin(Login, Password) ? TradeEntities.GetContext().User.First(b => b.UserLogin == Login && b.UserPassword == Password).UserRole : 0;

    }
}

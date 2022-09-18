using System.Linq;

namespace Flowers.Model.Classes
{
    internal class CheckUserRoleClass
    {
        public static int CheckUserRole(string Login, string Password)
        {
            if (CheckLoginClass.CheckLogin(Login, Password))
            {
                UserIDClass.UID = TradeEntities.GetContext().User
                    .First(b => b.UserLogin == Login && b.UserPassword == Password).UserID;
                return TradeEntities.GetContext().User.First(b => b.UserLogin == Login && b.UserPassword == Password)
                    .UserRole;
            }
            else
                return 0;

        }
    }
}

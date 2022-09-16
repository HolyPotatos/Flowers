using System.Linq;

namespace Flowers.Model.Classes
{
    internal class CheckLoginClass
    {
        public static bool CheckLogin(string Login, string Password) => TradeEntities.GetContext().User.Any(b => b.UserLogin == Login && b.UserPassword == Password);
    }
}

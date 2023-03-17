using Microsoft.EntityFrameworkCore;
using Proje.Models.MVVM;

namespace Proje.Models
{
    public class cls_User
    {
        iakademi45Context context = new iakademi45Context();

        public async Task<User> loginControl(User user)
        {
            User? usr = await context.Users.FirstOrDefaultAsync
                (u => u.Email == user.Email && u.Password == user.Password && u.IsAdmin == true && u.Active == true);
            return usr;
        }

    }
}

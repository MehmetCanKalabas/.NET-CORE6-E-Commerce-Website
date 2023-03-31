using Microsoft.EntityFrameworkCore;
using Proje.Models.MVVM;
using System.Text;
using XSystem.Security.Cryptography;

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

        public static User SelectMemberInfo(string email)
        {
            using (iakademi45Context context = new iakademi45Context())
            {
                User usr = context.Users.FirstOrDefault(u => u.Email == email);
                return usr;
            }
        }
        public static string MemberControl(User user)
        {
            using (iakademi45Context context = new iakademi45Context())
            {
                string answer = "";
                try
                {
                    string md5Sifre = MDSifrele(user.Password);
                    User usr = context.Users.FirstOrDefault(u => u.Email == user.Email & u.Password == md5Sifre);
                    if (usr == null)
                    {
                        //kullanıcı yanlış şifre veya email girdi
                        answer = "error";
                    }
                    else
                    {
                        //kullanıcı veritabanında kayıtlı
                        if (usr.IsAdmin == true)
                        {
                            //admin yetkisi olan personel
                            answer = "admin";
                        }
                        else
                        {
                            answer = usr.Email;
                        }
                    }
                }
                catch (Exception)
                {

                    return "HATA";
                }
                return answer;
            }
        }

        public static string MDSifrele(string value)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] btr = Encoding.UTF8.GetBytes(value);
            btr = md5.ComputeHash(btr);

            StringBuilder sb = new StringBuilder();
            foreach (byte item in btr)
            {
                sb.Append(item.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
    }
}

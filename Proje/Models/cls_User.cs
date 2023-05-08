using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proje.Models.MVVM;
using System.Net;
using System.Net.Mail;
using System.Text;
using XSystem.Security.Cryptography;

namespace Proje.Models
{
    public class cls_User
    {
        iakademi45Context context = new iakademi45Context();

        public async Task<User> loginControl(User user)
        {
            string md5Sifre = MDSifrele(user.Password);
            User? usr = await context.Users.FirstOrDefaultAsync
                (u => u.Email == user.Email && u.Password == md5Sifre && u.IsAdmin == true && u.Active == true);
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
        public static User findbyıd(int id)
        {
            using (iakademi45Context context = new iakademi45Context())
            {
                User usr = context.Users.FirstOrDefault(u => u.UserID == id);
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

        public static bool AddUser(User user)
        {
            using (iakademi45Context context = new iakademi45Context())
            {
                try
                {
                    user.Active = true;
                    user.IsAdmin = false;
                    user.Password = MDSifrele(user.Password);
                    context.Users.Add(user);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public static bool loginEmailControl(User user)
        {
            using(iakademi45Context context = new iakademi45Context())
            {
               User usr = context.Users.FirstOrDefault(u => u.Email == user.Email);
               if (usr == null)
               {
                  return false;
               }
               return true;
            }
        }

        public static void Send_Sms(string OrderGroupGUID)
        {
            using (iakademi45Context context = new iakademi45Context())
            {
                string ss = "";
                ss += "<?xml version = '1.0' encoding = 'UTF-8'>";
                ss += "<mainbody>";
                ss += "<header>";
                ss += "<company dil = 'TR'> iakademi(üye olununca size verilen şirket ismi)</company>";
                ss += "<usercode>0850 bize verilen user kod buraya yazılacak</usercode>";
                ss += "<password>NetGsm123. verilen şifre buraya yazılacak</password>";
                ss += "<startdate></startdate>"; // hiç bir şey belirtilmezse mesaj o an gidiyor.
                ss += "<stopdate></stopdate>";
                ss += "<type>n:n</type>"; //kaç kullanıcıya gidecek
                ss += "<msgheader></msgheader>"; // mesaj baslıgı
                ss += "</header>";
                ss += "<body>";
                
                Order order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);
                User user = context.Users.FirstOrDefault(u => u.UserID == order.UserID);
                //Sayın Mehmet Can Kalabaş, 05/04/2023 tarihinde 50215987354 nolu siparişiniz alınmıştır.
                string content = "Sayın " + user.NameSurname + "," + DateTime.Now + " tarihinde " + OrderGroupGUID + " nolu siparişiniz alınmıştır.";

                ss += "<mp><msg><![CDATA[" + content + "]]></msg><no>90" + user.Telephone + "</no></mp>";
                ss += "</body>";
                ss += "</mainbody>";

                string answer = XMLPOST("http://api.netgsm.com/tr/xmlbulkhttppost.asp", ss);
                if (answer != "-1")
                {
                    //sms gitti
                }
                else
                {
                    //sms gitmedi
                }         
            }
        }

        public static string XMLPOST(string url ,string xmlData)
        {
            try
            {
                WebClient wUpload = new WebClient();
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; //Convert = CASTING
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                Byte[] bResponse = wUpload.UploadData(url, "POST", bPostArray);

                Char[] sReturnsChars = Encoding.UTF8.GetChars(bResponse);

                string sWebPage = new string(sReturnsChars);
                return sWebPage;
            }
            catch (Exception)
            {

                return "-1";
            }
        }

        public static void Send_Email(string OrderGroupGUID)
        {
            using (iakademi45Context context = new iakademi45Context())
            {
                Order order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);
                User user = context.Users.FirstOrDefault(u => u.UserID == order.UserID);

                string mail = "gonderen email buraya info@can.com"; //dinamik olmalı tablodan gelecek
                string _mail = user.Email;
                string subject = "";
                string content = "";

                content = "Sayın " + user.NameSurname + "," + DateTime.Now + " tarihinde " + OrderGroupGUID + " nolu siparişiniz alınmıştır.";

                subject = "Sayın " + user.NameSurname + " siparişiniz alınmıştır.";

                string host = "smtp.iakademi.com"; //hangi sunucudan
                int port = 555; //hangi portla
                string login = "mailservera bağlanılan login";
                string password = "mailservera bağlanılan şifre";

                MailMessage e_posta = new MailMessage();
                e_posta.From = new MailAddress(mail, "can bilgi"); // gönderen
                e_posta.To.Add(_mail); // alıcı
                e_posta.Subject = subject;
                e_posta.IsBodyHtml = true;
                e_posta.Body = content;

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential(login,password);
                smtp.Host = host;
                smtp.Port = port;

                try
                {
                    smtp.Send(e_posta);
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
        }

        //public async Task<User> ProfileEdit(User user)
        //{
            
        //    return result;
        //}

    }
}

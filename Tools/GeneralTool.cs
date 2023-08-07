using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Divisima.Tools
{
    public class GeneralTool
    {
        public static string getMD5(string _text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(_text));
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        public static string URLConvert(string _text) 
        {
            return _text.ToLower().Replace(" ", "-").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ç", "c").Replace("ğ", "g").Replace("ş", "s");
        }

        public static void MailGonder(string kime, string konu, string mesaj)
        {
            SmtpClient smtpClient = new();
            smtpClient.Host = "mail.biltekno.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = false;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("test@biltekno.com", "t123e");
            MailMessage mailMessage = new();
            mailMessage.From = new MailAddress("test@biltekno.com");
            mailMessage.To.Add(kime);
            mailMessage.Subject = konu;
            mailMessage.Body = mesaj;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;
            smtpClient.Send(mailMessage);
        }
    }
}

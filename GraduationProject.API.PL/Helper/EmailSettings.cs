using GraduationProject.API.DAL.Models.IdentityModels;
using System.Net.Mail;
using System.Net;

namespace GraduationProject.API.PL.Helper
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("zeyadenab220@gmail.com", "vzvkdmavvsyiwtmp");
            client.Send("zeyadenab220@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}

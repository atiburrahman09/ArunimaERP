using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace ScopoERP.Web.Helper
{
    public class MailHelper
    {
        public async Task<bool> SendMessage(string recipient, string subject, string mailBody)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(recipient));
            message.Subject = subject;
            message.Body = string.Format(mailBody);
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);

                return true;
            }
        }
    }

    public class MailViewModel
    {
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string MailBody { get; set; }
    }
}
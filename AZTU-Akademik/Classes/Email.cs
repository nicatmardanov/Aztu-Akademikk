using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace AZTU_Akademik.Classes
{
    public class Email
    {
        public static async Task SendEmail(string from, string to, string subject, string html)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };


            using SmtpClient smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync("[USERNAME]", "[PASSWORD]").ConfigureAwait(false);
            await smtp.SendAsync(email).ConfigureAwait(false);
            await smtp.DisconnectAsync(true).ConfigureAwait(false);
        }

    }
}

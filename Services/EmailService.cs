using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

using QuPic.Services.Interfaces;

public class EmailService : IEmailService
{
    public async Task SendEmailWithAttachAsync(string to, string subject, string body, byte[] attach)
    {
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse("qupic.rosario.cavite@gmail.com"));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        var builder = new BodyBuilder
        {
            TextBody = body
        };

        if (attach != null)
        {
            builder.Attachments.Add("qrimg", attach, ContentType.Parse("image/png"));
        }

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync("qupic.rosario.cavite@gmail.com", "ejac qvyl bbfx wyyl"); // HARDCODED FOR NOW ** LESS SECURE 

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);
    }

}
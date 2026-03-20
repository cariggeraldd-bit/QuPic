namespace QuPic.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailWithAttachAsync(string to, string subject, string body, byte[] attach);
    }
}
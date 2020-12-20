namespace Veri.System.Services
{
    public interface IMailService
    {
        string SendMessage(string to, string subject, string body);
    }
}

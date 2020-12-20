using System.Threading.Tasks;

namespace Veri.System.Services
{
    public interface ITextService
    {
        Task SendMessageAsync(string from, string to, string message);
    }
}

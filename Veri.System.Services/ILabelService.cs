namespace Veri.System.Services
{
    public interface ILabelService
    {

        string Get(string code, string iso = "en");
        string Get(string code, string iso, params string[] inputs);
    }
}

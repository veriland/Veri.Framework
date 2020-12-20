namespace Veri.System.Data
{
    public interface IBaseTable
    {
        ICacheContext Cache { get; set; }

        void Save(int ttl = -1);

        bool Validate();

        void Delete();

        void Insert(int ttl = -1);

        SysRequestLog RequestLog(string operation, string method = "GET", string identity = "N/A", string message = "success");
    }
}

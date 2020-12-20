using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

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
   
    public abstract class BaseTable : IBaseTable
    {
        [IgnoreDataMember]
        private ICacheContext cache;

        [IgnoreDataMember]
        public ICacheContext Cache
        {
            get
            {
                return cache;
            }
            set
            {
                cache = value;
            }
        }

        public BaseTable(ICacheContext cache)
        {
            this.cache = cache;
        }

        public virtual void Save(int ttl = -1)
        {
            if (Validate())
            {
                MethodInfo mi = typeof(CacheContext).GetMethod("Store");
                mi = mi.MakeGenericMethod(this.GetType());
                mi.Invoke(Cache, new object[] { this, ttl });
            }
        }

        public virtual bool Validate()
        {
            bool ret = true;

            return ret;
        }

        public virtual void Delete()
        {
            Cache.Delete(this);
        }

        public virtual void Insert(int ttl = -1)
        {
            Save(ttl);
        }

        public SysRequestLog RequestLog(string operation, string method = "GET", string identity = "N/A", string message = "success")
        {

            var sysRequestLog = new SysRequestLog(cache)
            {
                Id = cache.GetDb<SysRequestLog>().GetNextSequence(),
                Identity = identity,
                Message = message,
                Method = method,
                Operation = operation,
                TransDate = DateTime.Now,
                Device = null
            };

            return sysRequestLog;
        }
    }
}

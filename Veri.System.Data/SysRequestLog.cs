using System;
using System.Collections.Generic;
using System.Text;
using Veri.System.Data;

namespace Veri.System.Data
{
    public class SysRequestLog : BaseTable
    {
        public SysRequestLog(ICacheContext cache) : base(cache)
        {
        }
        public long Id { get; set; }
        public string Identity { get; set; }
        public string IdentityType { get; set; }
        public DateTime TransDate { get; set; }
        public string Operation { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }
        public SysDevice Device { get; set; }
    }
   

    public class SysDevice
    {
        public string Model { get; set; }
        public string Make { get; set; }
        public string OS { get; set; }
    }
}

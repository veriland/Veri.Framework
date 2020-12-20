using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Veri.System.Data;
using Veri.System.Services;

namespace Veri.System.Api
{
    [Authorize]
    public class VeriController : ControllerBase
    {
        protected ICacheContext cache;
        protected ITokenService tokenService;
        protected ILabelService label;

        public string Claim
        {
            get
            {
                return HttpContext.User.Identity.Name;
            }
        }
        public VeriController(ICacheContext cache, ITokenService tokenService, ILabelService label)
        {
            this.cache = cache;
            this.tokenService = tokenService;
            this.label = label;
        }

        public SysRequestLog RequestLog(Exception exception, string operation, string method = "GET", string identity = "N/A")
        {
            return RequestLog(operation, method, identity, exception.ToString());
        }

        public SysRequestLog RequestLog(string operation, string method = "GET", string identity = "N/A", string message = "success", string identityType = "Person")
        {

            SysDevice sysDevice = new SysDevice();
            if (Request.Headers.ContainsKey("X-DEVICE-MAKE"))
            {
                sysDevice.Make = Request.Headers["X-DEVICE-MAKE"];
            }

            if (Request.Headers.ContainsKey("X-DEVICE-MODEL"))
            {
                sysDevice.Model = Request.Headers["X-DEVICE-MODEL"];
            }

            if (Request.Headers.ContainsKey("X-DEVICE-OS"))
            {
                sysDevice.OS = Request.Headers["X-DEVICE-OS"];
            }

            var sysRequestLog = new SysRequestLog(cache)
            {
                Id = cache.GetDb<SysRequestLog>().GetNextSequence(),
                Identity = identity,
                IdentityType = identityType,
                Message = message,
                Method = method,
                Operation = operation,
                TransDate = DateTime.Now,
                Device = sysDevice
            };

            return sysRequestLog;
        }

        protected string GenerateToken(string claim)
        {
            return tokenService.CreateToken(claim);
        }

        protected string Label(string str)
        {
            return label.Get(str);
        }

        protected string Label(string str, params string[] inputs)
        {
            return label.Get(str, "en", inputs);
        }

    }
}

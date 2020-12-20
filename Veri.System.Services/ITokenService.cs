using System;

namespace Veri.System.Services
{
    public interface ITokenService
    {
        string CreateToken(string claim);
    }
}

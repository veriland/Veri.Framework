using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Veri.System.Extensions
{
    public static class PasswordExtensions
    {
        public static bool IsUsernameStrong(this string str)
        {
            return (str.Length >= 4) && !str.Contains(" ");
        }
        public static bool IsPasswordStrong(this string str)
        {
            var pattern = @"^(?=(.*\d){1})(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,}$";
            var match = Regex.Match(str, pattern);
            return match.Success && (match.Value == str);
        }
        public static bool IsPinStrong(this string str)
        {
            var pattern = @"^[0-9]{6,6}$";
            var match = Regex.Match(str, pattern);
            return match.Success && (match.Value == str);
        }
    }
}

using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Veri.System.Extensions
{
    public static class EmailAddressExtensions
    {
        public static string ToMaskedEmail(this string str)
        {
            int left = 1;
            int right = 3;
            if (str.IsValidEmail())
            {
                var match = Regex.Match(str, @"[^@]*");
                if (match != null && !string.IsNullOrEmpty(match.Value))
                {
                    var size = match.Value.Length;
                    if (size <= 4)
                    {
                        left = right = 0;
                    }
                    else
                    {
                        left = right = 1;
                    }
                }
                var pattern = $@"(?<=[\w]{{{left}}})[\w-\._\+%]*(?=[\w]{{{right}}}@)";
                str = Regex.Replace(str, pattern, m => new string('*', m.Length));
            }
            return str;
        }

        public static bool IsValidEmail(this string str)
        {
            try
            {
                var addr = new MailAddress(str);
                return addr.Address == str;
            }
            catch
            {
                return false;
            }
        }
    }
}

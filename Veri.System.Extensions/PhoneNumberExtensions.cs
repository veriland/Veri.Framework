using PhoneNumbers;
using System;
using System.Text.RegularExpressions;

namespace Veri.System.Extensions
{
    public static class PhoneString
    {
        static readonly PhoneNumberUtil PhoneNumberUtil = PhoneNumberUtil.GetInstance();
        private static PhoneNumber Parse(this string str)
        {
            if (str == null)
            {
                return null;
            }
            str = Regex.Replace(str, "[^+0-9]", "");
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            try
            {
                if (!str.Contains("+"))
                {
                    var numberProto = PhoneNumberUtil.Parse(str, "GB");
                    return numberProto;
                }
                else
                {
                    var numberProto = PhoneNumberUtil.Parse(str, "");
                    return numberProto;
                }
            }
            catch (NumberParseException)
            {
                return null;
            }
        }
        public static string ToPhoneNumberNational(this string str)
        {
            var phoneNumber = str.Parse();
            if (phoneNumber == null)
            {
                return null;
            }
            else
            {
                return PhoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.E164);
            }
        }

        /// <summary>
        /// Converts string to E.164 phone number format with country prefix +44
        /// </summary>
        /// <param name="str">Phone number string</param>
        /// <returns>E.164 phone number format with country prefix +44</returns>
        public static string ToPhoneNumberIntl(this string str)
        {
            var phoneNumber = str.Parse();
            if(phoneNumber == null)
            {
                return null;
            }
            else
            {
                return PhoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
            }
        }


        public static string ToPhoneNumber(this string str)
        {
            var numberProto = str.Parse();
            if(numberProto == null)
            {
                return null;
            }
            else
            {
                return PhoneNumberUtil.Format(numberProto, PhoneNumberFormat.E164);
            }
        }
        public static int GetPhoneNumberType(this string str)
        {
            var phoneNumber = str.Parse();
            if (phoneNumber == null)
            {
                return -1;
            }
            else
            {
                var numberType = PhoneNumberUtil.GetNumberType(phoneNumber);
                switch(numberType)
                {
                    case PhoneNumberType.FIXED_LINE:
                        return 0;
                    case PhoneNumberType.FIXED_LINE_OR_MOBILE:
                        return 0;
                    case PhoneNumberType.MOBILE:
                        return 1;
                    default:
                        return -1;
                }
            }

        }
        public static bool IsLandlineNumber(this string str)
        {
            var phoneNumber = str.Parse();
            if(phoneNumber == null)
            {
                return false;
            }
            else
            {
                return str.GetPhoneNumberType() == 0;
            }
        }
        public static string NumberNotAllowed(this string str)
        {
            return str.GetPhoneNumberTypeText();
        }
        public static string GetPhoneNumberTypeText(this string str)
        {
            if (str == null)
            {
                return null;
            }
            str = Regex.Replace(str, "[^+0-9]", "");
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            str = Regex.Replace(str, "[^+0-9]", "");
            var numberProto = PhoneNumberUtil.ParseAndKeepRawInput(str, "GB");
            var numberType = PhoneNumberUtil.GetNumberType(numberProto);

            switch (numberType)
            {
                case PhoneNumberType.PAGER:
                    return "pager";

                case PhoneNumberType.PERSONAL_NUMBER:
                    return "personal";

                case PhoneNumberType.PREMIUM_RATE:
                    return "premium rate";

                case PhoneNumberType.SHARED_COST:
                    return "shared cost";

                case PhoneNumberType.TOLL_FREE:
                    return "toll free";

                case PhoneNumberType.UAN:
                    return "universal account";

                case PhoneNumberType.UNKNOWN:
                    return "unknown";

                case PhoneNumberType.VOICEMAIL:
                    return "voicemail";

                case PhoneNumberType.VOIP:
                    return "voip";

                default:
                    return null;
            }
        }
        public static bool IsMobilePhoneNumber(this string str)
        {
            var numberProto = str.Parse();
            if(numberProto == null)
            {
                return false;
            }
            else
            {
                var numberType = PhoneNumberUtil.GetNumberType(numberProto);
                return (numberType == PhoneNumberType.MOBILE);
            }
        }
        public static bool IsViablePhoneNumber(this string str)
        {
            if (str == null)
            {
                return false;
            }
            str = Regex.Replace(str, "[^+0-9]", "");
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            str = Regex.Replace(str, "[^+0-9]", "");
            try
            {

                var numberProto = PhoneNumberUtil.ParseAndKeepRawInput(str, "GB");
                return PhoneNumberUtil.IsValidNumber(numberProto);
            }
            catch (NumberParseException)
            {
                return false;
            }
        }
        public static string ToMaskedPhone(this string str)
        {
            var phoneNumber = str.Parse();
            if (phoneNumber == null)
            {
                return null;
            }
            else
            {
                var number = phoneNumber.NationalNumber.ToString();
                var pattern = @"(?<=[\w]{1})[\w-\._ \+%]*(?=[\w]{4})";
                if (phoneNumber.CountryCode == 44)
                {
                    number = Regex.Replace(number, pattern, m => new string('*', m.Length));
                    number = $"0{number}";
                }
                else
                {
                    number = Regex.Replace(number, pattern, m => new string('*', m.Length));
                    number = $"+{phoneNumber.CountryCode}{number}";
                }
                return number;
            }
        }
    }
}

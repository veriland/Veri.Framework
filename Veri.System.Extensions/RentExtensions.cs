using System;
using System.Collections.Generic;
using System.Text;

namespace Veri.System.Extensions
{
    public static class RentExtensions
    {
        public static decimal ToWeeklyCharge(this decimal rent, string period = "Weekly")
        {
            decimal weeks = 52;
            switch (period)
            {
                case "Weekly":
                    return rent;

                case "Annual":
                    return decimal.Divide(rent, weeks);

                case "Fortnightly":
                    return decimal.Divide(rent, 2);

                case "Monthly":
                    return decimal.Divide(rent * 12, weeks);

                case "Quarterly":
                    return decimal.Divide(rent * 4, weeks);

                default:
                    return 0;
            }
        }

        public static decimal ToMonthlyCharge(this decimal rent, string period = "Monthly")
        {
            decimal weeks = 52;
            switch (period)
            {
                case "Weekly":
                    return decimal.Divide(rent * weeks, 12);

                case "Annual":
                    return decimal.Divide(rent, 12);

                case "Fortnightly":
                    return decimal.Divide(rent * weeks, 12) * 2;

                case "Monthly":
                    return rent;

                case "Quarterly":
                    return decimal.Divide(rent * weeks, 12) * 4;

                default:
                    return 0;
            }
        }
        public static decimal ToFortnightlyCharge(this decimal rent, string period = "Fortnightly")
        {
            decimal weeks = 52;
            switch (period)
            {
                case "Weekly":
                    return rent * 2;

                case "Annual":
                    return rent / weeks * 2;

                case "Fortnightly":
                    return rent;

                case "Monthly":
                    return rent * 12 / weeks * 2;

                case "Quarterly":
                    return rent / 3 * 12 / weeks * 2;

                default:
                    return 0;
            }
        }

        public static decimal ToFourWeekly(this decimal rent, string period = "Weekly")
        {
            decimal weeks = 52;
            switch (period)
            {
                case "Weekly":
                    return rent * 4;

                case "Annual":
                    return rent / weeks * 4;

                case "Fortnightly":
                    return rent * 2;

                case "Monthly":
                    return rent * 12 / weeks * 4;

                case "Quarterly":
                    return rent / 3 * 12 / weeks * 4;

                default:
                    return 0;
            }
        }
    }
}

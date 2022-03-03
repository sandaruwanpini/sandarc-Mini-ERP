using System;

namespace ERP.CSharpLib
{
    public sealed class UTCDateTime
    {
        private static DateTime srilankatime()
        {
            DateTime localTime;
            try
            {
                localTime = DateTime.Now;
            }
            catch (Exception)
            {
                localTime = DateTime.Now;
            }

            return localTime;
        }
        public static DateTime BDDateTime()
        {
            return srilankatime();
        }
        public static TimeSpan BDTime()
        {
            return srilankatime().TimeOfDay;
        }
        public static DateTime BDDate()
        {
            return srilankatime().Date;
        }
    }


}

using System.Globalization;

namespace Calendar;

public static class CultureHelper
{
    public static void SetAllCultures(CultureInfo cultureInfo)
    {
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }
}
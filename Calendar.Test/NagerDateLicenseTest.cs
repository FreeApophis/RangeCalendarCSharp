using Nager.Date;
using Xunit;

namespace Calendar.Test;

/// <summary>
/// License keys for OpenSource Projects are the most stupid idea so far.
/// </summary>
public class NagerDateLicenseTest
{
    [Fact]
    public void GivenTheKeyTheLicenseValid()
    {
        DateSystem.LicenseKey = "LostTimeIsNeverFoundAgain";

        _ = DateSystem.GetPublicHolidayProvider(CountryCode.CH);
    }
}
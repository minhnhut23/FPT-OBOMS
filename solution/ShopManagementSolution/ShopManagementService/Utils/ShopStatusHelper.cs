namespace ShopManagementService.Utils;

public class ShopStatusHelper
{
    public static string GetShopStatus(TimeSpan openingTime, TimeSpan closingTime)
    {
        TimeSpan now = DateTime.Now.TimeOfDay;

        if (now >= openingTime && now <= closingTime)
            return "Is opening";

        return "Closed";
    }
}

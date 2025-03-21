using Microsoft.Extensions.Logging;

namespace Services;

public sealed class TollDateService(
    ILogger<TollDateService> logger, 
    HolidaysService holidaysService
)
{
    private readonly HashSet<int> _validYears = [2013];

    public bool IsTollFreeDate(DateTime dateTime) => 
        IsTollFreeDate(DateOnly.FromDateTime(dateTime));

    public bool IsTollFreeDate(DateOnly date)
    {
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            logger.LogDebug("Date {Date} is a weekend.", date);
            return true;
        }

        if (_validYears.Contains(date.Year) == false)
        {
            logger.LogDebug("Date {Date} is not in the year 2013.", date);
            return false;
        }

        if (date.Month == 7)
        {
            logger.LogDebug("Date {Date} is in July.", date);
            return true;
        }

        return holidaysService.IsHoliday(date);
    }
}
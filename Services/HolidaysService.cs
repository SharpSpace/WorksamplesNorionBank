using Microsoft.Extensions.Logging;
using Repository;

namespace Services;

public sealed class HolidaysService(
    ILogger<HolidaysService> logger, 
    Holidays holidaysRepository
)
{
    private readonly HashSet<DateOnly> _holidays = holidaysRepository.GetAll();

    public bool IsHoliday(DateOnly date)
    {
        var isTollFree = _holidays.Contains(date);
        if (isTollFree)
        {
            logger.LogDebug("Date {Date} is holiday: {IsTollFree}", date, isTollFree);
        }
        else
        {
            isTollFree = _holidays.Select(x => x.AddDays(-1)).Contains(date);
            logger.LogDebug("Date before {Date} is holiday: {IsTollFree}", date, isTollFree);
        }

        return isTollFree;
    }
}
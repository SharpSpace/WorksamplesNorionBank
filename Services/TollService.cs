using Microsoft.Extensions.Logging;
using Models.Interfaces;

namespace Services;

public sealed class TollService(
    ILogger<TollService> logger,
    int maxFee,
    VehicleService vehicleService,
    TollDateService tollDateService
)
{
    /// <summary>
    /// Calculate the total toll fee for one day
    /// </summary>
    /// <param name="vehicle">The vehicle</param>
    /// <param name="dates">Date and time of all passes on one day</param>
    /// <returns>The total toll fee for that day</returns>
    public int GetTollFee(IVehicle? vehicle, List<DateTime> dates)
    {
        if (vehicle == null)
        {
            logger.LogDebug("Vehicle is null, returning 0.");
            return 0;
        }

        logger.LogDebug(
            "Calculating total toll fee for vehicle: {VehicleType} and dates: {Dates}",
            vehicle.GetVehicleType(),
            dates
        );

        if (vehicleService.IsTollFree(vehicle)) return 0;

        var totalFee = CalculateTotalFee(dates);
        logger.LogDebug("Total calculated fee: {TotalFee}", totalFee);

        return totalFee;
    }

    private int CalculateTotalFee(IEnumerable<DateTime> dates)
    {
        var totalFee = 0;
        var groupedDates = DateService.GroupDates(dates, TimeSpan.FromMinutes(60));

        foreach (var fee in groupedDates.Select(x => x.Max(GetTollFee)))
        {
            totalFee += fee;
            if (totalFee <= maxFee) continue;

            logger.LogDebug("Total fee exceeds {MaxFee}, returning {MaxFee}.", maxFee, maxFee);
            return maxFee;
        }

        return totalFee;
    }

    private int GetTollFee(DateTime date)
    {
        logger.LogDebug("Calculating toll fee for date: {Date}", date);

        if (tollDateService.IsTollFreeDate(date))
        {
            logger.LogDebug("Date is toll-free.");
            return 0;
        }

        var fee = TollTimeService.GetToll(date);

        logger.LogDebug("Calculated fee: {Fee}", fee);
        return fee;
    }
}
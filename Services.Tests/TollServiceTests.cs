using Models;
using Moq;
using Repository;

namespace Services.Tests;

[TestClass]
public sealed class TollServiceTests
{
    private TollService _tollService = null!;

    [TestMethod]
    public void GetTollFee_Holiday_ReturnsZero()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 12, 25, 6, 15, 0); // Christmas Day, 0 kr

        var fee = _tollService.GetTollFee(vehicle, [date]);

        Assert.AreEqual(0, fee);
    }

    [TestMethod]
    public void GetTollFee_MaximumDailyFee_Returns60()
    {
        var vehicle = new Car();
        var dates = new List<DateTime>
        {
            new(2013, 10, 10, 6, 15, 0), // 8 kr
            new(2013, 10, 10, 7, 15, 1), // 18 kr
            new(2013, 10, 10, 8, 15, 2), // 13 kr
            new(2013, 10, 10, 15, 15, 0), // 13 kr
            new(2013, 10, 10, 16, 15, 1), // 18 kr
            new(2013, 10, 10, 17, 15, 2)  // 13 kr
        };

        var fee = _tollService.GetTollFee(vehicle, dates);

        Assert.AreEqual(60, fee);
    }

    [TestMethod]
    public void GetTollFee_MultiplePassagesExceeding60Minutes_ReturnsSumOfFees()
    {
        var vehicle = new Car();
        var dates = new List<DateTime>
        {
            new(2013, 10, 10, 6, 15, 0), // 8 kr
            new(2013, 10, 10, 7, 15, 1)  // 18 kr
        };

        var fee = _tollService.GetTollFee(vehicle, dates);

        Assert.AreEqual(26, fee);
    }

    [TestMethod]
    public void GetTollFee_MultiplePassagesInDifferentTimeSlots_ReturnsCorrectSum()
    {
        var vehicle = new Car();
        var dates = new List<DateTime>
        {
            new(2013, 10, 10, 8, 15, 0), // 13 kr
            new(2013, 10, 10, 10, 15, 0), // 8 kr
            new(2013, 10, 10, 15, 15, 0) // 13 kr
        };

        var fee = _tollService.GetTollFee(vehicle, dates);

        Assert.AreEqual(34, fee);
    }

    [TestMethod]
    public void GetTollFee_MultiplePassagesWithin60Minutes_ReturnsHighestFee()
    {
        var vehicle = new Car();
        var dates = new List<DateTime>
        {
            new(2013, 10, 10, 6, 15, 0), // 8 kr
            new(2013, 10, 10, 6, 45, 0),  // 13 kr
            new(2013, 10, 10, 15, 15, 0) // 13 kr
        };

        var fee = _tollService.GetTollFee(vehicle, dates);

        Assert.AreEqual(26, fee);
    }

    [TestMethod]
    public void GetTollFee_PassagesInJuly_ReturnsZero()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 7, 10, 6, 15, 0); // July, 0 kr

        var fee = _tollService.GetTollFee(vehicle, [date]);

        Assert.AreEqual(0, fee);
    }

    [TestMethod]
    public void GetTollFee_PassagesOnDayBeforeHoliday_ReturnsZero()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 12, 23, 6, 15, 0); // Day before Christmas, 0 kr

        var fee = _tollService.GetTollFee(vehicle, [date]);

        Assert.AreEqual(0, fee);
    }

    [TestMethod]
    public void GetTollFee_PassagesOnHoliday_ReturnsZero()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 12, 24, 6, 15, 0); // Christmas Eve, 0 kr

        var fee = _tollService.GetTollFee(vehicle, [date]);

        Assert.AreEqual(0, fee);
    }

    [TestMethod]
    public void GetTollFee_SinglePassage_ReturnsCorrectFee()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 10, 10, 6, 15, 0); // 8 kr

        var fee = _tollService.GetTollFee(vehicle, [date]);

        Assert.AreEqual(8, fee);
    }

    [TestMethod]
    public void GetTollFee_TollFreeVehicle_ReturnsZero()
    {
        var vehicle = new Motorbike();
        var date = new DateTime(2013, 10, 10, 6, 15, 0); // 06:15, should be 0 kr for motorbike

        var fee = _tollService.GetTollFee(vehicle, [date]);

        Assert.AreEqual(0, fee);
    }

    [TestMethod]
    public void GetTollFee_Weekend_ReturnsZero()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 10, 12, 6, 15, 0); // Saturday, 0 kr

        var fee = _tollService.GetTollFee(vehicle, [date]);

        Assert.AreEqual(0, fee);
    }

    [TestInitialize]
    public void Setup()
    {
        var mockHolidays = new Mock<Holidays>();
        var mockVehicleTypes = new Mock<VehicleTypes>();

        mockHolidays.Setup(h => h.GetAll()).Returns(
        [
            new DateOnly(2013, 12, 25), // Christmas Day
            new DateOnly(2013, 12, 24), // Christmas Eve
            new DateOnly(2013, 12, 23) // Day before Christmas
        ]);

        mockVehicleTypes.Setup(v => v.GetTollFree()).Returns(
        [
            nameof(Common.VehicleTypes.Motorbike),
            nameof(Common.VehicleTypes.Tractor),
            nameof(Common.VehicleTypes.Emergency),
            nameof(Common.VehicleTypes.Diplomat),
            nameof(Common.VehicleTypes.Foreign),
            nameof(Common.VehicleTypes.Military)
        ]);

        var holidaysService = new HolidaysService(
            LoggerConfig.CreateLogger<HolidaysService>(), 
            mockHolidays.Object
        );
        var vehicleService = new VehicleService(
            LoggerConfig.CreateLogger<VehicleService>(),
            mockVehicleTypes.Object
        );

        var tollDateService = new TollDateService(LoggerConfig.CreateLogger<TollDateService>(), holidaysService);

        _tollService = new TollService(
            LoggerConfig.CreateLogger<TollService>(),
            60,
            vehicleService,
            tollDateService
        );
    }
}
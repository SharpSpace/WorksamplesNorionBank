using Microsoft.Extensions.Logging;
using Models.Interfaces;
using Repository;

namespace Services;

public sealed class VehicleService(ILogger<VehicleService> logger, VehicleTypes vehicleTypesRepository)
{
    private readonly HashSet<string> _tollFreeVehicleTypes = vehicleTypesRepository.GetTollFree();

    public bool IsTollFree(IVehicle vehicle)
    {
        var vehicleType = vehicle.GetVehicleType();
        var isTollFree = _tollFreeVehicleTypes.Contains(vehicleType);
        
        logger.LogDebug("Vehicle {VehicleType} is toll-free: {IsTollFree}", vehicleType, isTollFree);
        
        return isTollFree;
    }
}
using Models.Interfaces;

namespace Models;

public sealed class Car : IVehicle
{
    public string GetVehicleType() => nameof(Common.VehicleTypes.Car);
}
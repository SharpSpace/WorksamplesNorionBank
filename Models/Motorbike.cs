using Common;
using Models.Interfaces;

namespace Models;

public sealed class Motorbike : IVehicle
{
    public string GetVehicleType() => nameof(VehicleTypes.Motorbike);
}
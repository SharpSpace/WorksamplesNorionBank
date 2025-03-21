namespace Repository;

public class VehicleTypes
{
    public virtual HashSet<string> GetTollFree() =>
    [
        nameof(Common.VehicleTypes.Motorbike),
        nameof(Common.VehicleTypes.Tractor),
        nameof(Common.VehicleTypes.Emergency),
        nameof(Common.VehicleTypes.Diplomat),
        nameof(Common.VehicleTypes.Foreign),
        nameof(Common.VehicleTypes.Military)
    ];
}
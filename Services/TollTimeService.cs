namespace Services;

public static class TollTimeService
{
    public static int GetToll(DateTime dateTime) => GetToll(TimeOnly.FromDateTime(dateTime));

    public static int GetToll(TimeOnly time)
    {
        var minute = time.Minute;
        return time.Hour switch
        {
            6 when minute <= 29 => 8,
            6 => 13,
            7 => 18,
            8 when minute <= 29 => 13,
            8 => 8,
            9 => 8,
            10 => 8,
            11 => 8,
            12 => 8,
            13 => 8,
            14 => 8,
            15 when minute <= 29 => 13,
            15 => 18,
            16 => 18,
            17 => 13,
            18 when minute <= 29 => 8,
            _ => 0
        };
    }
}
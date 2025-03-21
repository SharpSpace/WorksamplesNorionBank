namespace Repository;

public class Holidays
{
    public virtual HashSet<DateOnly> GetAll() =>
    [
        new DateOnly(2013, 1, 1),
        new DateOnly(2013, 3, 28),
        new DateOnly(2013, 3, 29),
        new DateOnly(2013, 4, 1),
        new DateOnly(2013, 4, 30),
        new DateOnly(2013, 5, 1),
        new DateOnly(2013, 5, 8),
        new DateOnly(2013, 5, 9),
        new DateOnly(2013, 6, 5),
        new DateOnly(2013, 6, 6),
        new DateOnly(2013, 6, 21),
        new DateOnly(2013, 11, 1),
        new DateOnly(2013, 12, 24),
        new DateOnly(2013, 12, 25),
        new DateOnly(2013, 12, 26),
        new DateOnly(2013, 12, 31)
    ];
}
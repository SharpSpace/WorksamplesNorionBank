namespace Services;

public static class DateService
{
    public static IEnumerable<List<DateTime>> GroupDates(
        IEnumerable<DateTime> dates, 
        TimeSpan timeBetween
    )
    {
        var currentGroup = new List<DateTime>();
        var lastDate = DateTime.MinValue;

        foreach (var date in dates.OrderBy(d => d))
        {
            if (currentGroup.Count == 0 || date - lastDate <= timeBetween)
            {
                currentGroup.Add(date);
            }
            else
            {
                yield return currentGroup;
                currentGroup = [date];
            }

            lastDate = date;
        }

        if (currentGroup.Count > 0)
        {
            yield return currentGroup;
        }
    }
}
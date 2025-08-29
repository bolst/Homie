using Homie.Models.Finances;

namespace Homie.Extensions;

public static class CollectionExtensions
{
    public static double TotalBetweenDates(this IEnumerable<FinanceItem> transactions, DateTime start, DateTime end)
        => transactions
            .SelectMany(x => x.TimeSeries.Select(y => (x.Amount, TransactionType: x.Type, y)))
            .Where(x => x.Item3.OccurrenceDate.IsBetween(start, end))
            .Sum(x => x.Amount);


    public static IEnumerable<Tuple<DateTime, DateTime, double>> GetTimeSeries(this IEnumerable<FinanceItem> transactions, DateTime start, DateTime end)
    {
        List<Tuple<DateTime, DateTime, double>> retval = [];
        
        transactions = transactions
            // drop all transactions that are outside date range
            .Where(x => x.StartDate <= end || x.EndDate >= start)
            .ToList();
        
        var iDate = start.LastSunday();
        for (; iDate <= end; iDate = iDate.AddDays(7))
        {
            var dateStart = iDate;
            var dateEnd = dateStart.NextSaturday();
            
            var dateRangeSum = transactions
                .SelectMany(x => x.TimeSeries)
                .Where(x => x is not null && x.OccurrenceDate.IsBetween(dateStart, dateEnd))
                .Sum(x => x.ParentItem.Amount);
            
            retval.Add(new Tuple<DateTime, DateTime, double>(dateStart, dateEnd, dateRangeSum));
        }

        return retval;
    }
}
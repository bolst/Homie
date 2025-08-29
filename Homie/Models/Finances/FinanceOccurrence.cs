namespace Homie.Models.Finances;


public sealed record FinanceOccurrence
{
    public DateTime OccurrenceDate { get; init; }
    public double CumSum { get; init; }
    public FinanceItem ParentItem { get; set; }
}
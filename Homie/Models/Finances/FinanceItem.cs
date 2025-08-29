using MudBlazor;

namespace Homie.Models.Finances;

public sealed record FinanceItem
{
    public Guid Id { get; init; }
    public string Title { get; set; }
    public double Amount { get; set; }
    public string Frequency { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public DateRange? DateRange
    {
        get => StartDate is null || EndDate is null ? null : new(StartDate.Value, EndDate.Value);
        set
        {
            if (value is null) return;
            StartDate = value.Start;
            EndDate = value.End;
        }
    }

    public List<FinanceOccurrence> TimeSeries { get; set; } = [];
}
namespace Homie.HomeDb.Models.Bank;

public sealed record BankCharge
{
    public Guid Id { get; init; }

    public string Amount { get; init; }

    public double Value { get; init; }

    public string Payee { get; init; }

    public DateTime EmailDate { get; init; }

    public double PayeeTotal { get; init; }
    
    public double Total { get; init; }
}
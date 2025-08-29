using System.Diagnostics.CodeAnalysis;

namespace Homie.Types;

public sealed record FinanceItemType(string Type, string Title, string GroupTitle) : CustomTypeBase<string>(Type);


public sealed record FinanceItemTypes
{
    public static FinanceItemType Income { get; } = new("income", "Income", "Income");
    public static FinanceItemType Expense { get; } = new("expense", "Expense", "Expenses");
    
    public static IEnumerable<FinanceItemType> All { get; } = [Income, Expense];
    
    private static IReadOnlyDictionary<string, FinanceItemType> _lookupDict { get; } = All.ToDictionary(x => x.Type);
    public static bool TryGetValue(string type, [NotNullWhen(true)] out FinanceItemType? value) => _lookupDict.TryGetValue(type, out value);
}
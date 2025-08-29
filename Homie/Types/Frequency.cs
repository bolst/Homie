using System.Diagnostics.CodeAnalysis;

namespace Homie.Types;

public sealed record Frequency(string Value, string Color) : CustomTypeBase<string>(Value);


public sealed record Frequencies
{
    public static Frequency Once { get; } = new("Once", "#F4B85B");
    public static Frequency Weekly { get; } = new("Weekly", "#6EE6B9");
    public static Frequency BiWeekly { get; } = new("Bi-Weekly", "#4076F3");
    public static Frequency Monthly { get; } = new("Monthly", "#7C70F7");
    
    public static IEnumerable<Frequency> All { get; } = [Once, Weekly, BiWeekly, Monthly];
    
    private static IReadOnlyDictionary<string, Frequency> _lookupDict { get; } = All.ToDictionary(x => x.Value);
    public static bool TryGetValue(string freq, [NotNullWhen(true)] out Frequency? value) => _lookupDict.TryGetValue(freq, out value);
}
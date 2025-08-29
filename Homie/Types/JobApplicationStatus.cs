using System.Diagnostics.CodeAnalysis;

namespace Homie.Types;

public sealed record JobApplicationStatus(string Status, string Color) : CustomTypeBase<string>(Status);


public sealed record JobApplicationStatuses
{
    public static JobApplicationStatus Pending { get; } = new("Pending", "#F4B85B");
    public static JobApplicationStatus Rejected { get; } = new("Rejected", "#E35B66");
    public static JobApplicationStatus Interview { get; } = new("Interview", "#5197F8");
    public static JobApplicationStatus Offer { get; } = new("Offer", "#756BDF");
    
    public static IEnumerable<JobApplicationStatus> All { get; } = [Pending, Rejected, Interview, Offer];
    
    private static IReadOnlyDictionary<string, JobApplicationStatus> _lookupDict { get; } = All.ToDictionary(x => x.Status);
    public static bool TryGetValue(string status, [NotNullWhen(true)] out JobApplicationStatus? value) => _lookupDict.TryGetValue(status, out value);
}
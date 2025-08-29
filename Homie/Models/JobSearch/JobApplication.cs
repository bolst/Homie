namespace Homie.Models.Jobs;


public sealed record JobApplication
{
    public Guid Id { get; init; }

    public string? Position { get; set; }
    
    public string? Company { get; set; }

    public string? Industry { get; set; }
    
    public string? Location { get; set; }

    public DateTime? DatePosted { get; set; }

    public DateTime? DateApplied { get; set; }
    
    public string? Notes { get; set; }
    
    public string? Link { get; set; }
    
    public string Status { get; set; }


    public override string ToString() => $"{Position} ({Company})";
}
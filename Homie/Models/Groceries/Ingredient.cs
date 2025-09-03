namespace Homie.Models.Groceries;

public sealed record Ingredient
{
    public Guid Id { get; init; }
    public string Title { get; set; }
    
    public override string ToString() => Title;
}
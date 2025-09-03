namespace Homie.Models.Groceries;

public sealed record Recipe
{
    public Guid Id { get; init; }
    public string Title { get; set; }
    public List<RecipeIngredient> Ingredients { get; set; } = [];

    public override string ToString() => Title;
}
namespace Homie.Models.Groceries;

public sealed record RecipeIngredient
{
    public Guid IngredientId { get; init; }
    public string IngredientTitle { get; init; }
    public Recipe Recipe { get; set; }
    
    public override string ToString() => IngredientTitle;
}
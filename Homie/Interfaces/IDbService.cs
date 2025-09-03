using Homie.Types;

namespace Homie.Interfaces;


public interface IDbService
{
    #region Finances
    
    Task<IEnumerable<Models.Finances.FinanceItem>> GetFinancialsAsync(FinanceItemType type);
    Task AddFinanceItemAsync(Models.Finances.FinanceItem financeItem);
    Task UpdateFinanceItemAsync(Models.Finances.FinanceItem financeItem);
    Task DeleteFinanceItemAsync(Guid financeId);
    Task UpsertFinancialOccurrencesAsync(Guid financeId);
    
    #endregion
    
    
    
    #region Job Search
    
    Task<IEnumerable<Models.Jobs.JobApplication>> GetJobApplicationsAsync();
    Task AddJobApplicationAsync(Models.Jobs.JobApplication jobApplication);
    Task UpdateJobApplicationAsync(Models.Jobs.JobApplication jobApplication);
    Task DeleteJobApplicationAsync(Guid jobApplicationId);

    #endregion
    
    
    
    #region Groceries
    
    Task<IEnumerable<Models.Groceries.Recipe>> GetRecipesAsync();
    Task AddRecipeAsync(Models.Groceries.Recipe recipe, IEnumerable<Models.Groceries.Ingredient> ingredients);
    Task UpdateRecipeAsync(Models.Groceries.Recipe recipe, IEnumerable<Models.Groceries.Ingredient> ingredients);
    Task DeleteRecipeAsync(Models.Groceries.Recipe recipe);
    
    Task<IEnumerable<Models.Groceries.Ingredient>> GetIngredientsAsync();
    Task<IEnumerable<Models.Groceries.Ingredient>> GetIngredientsByRecipeIdAsync(Guid recipeId);
    Task AddIngredientAsync(Models.Groceries.Ingredient ingredient);
    Task UpdateIngredientAsync(Models.Groceries.Ingredient ingredient);
    Task DeleteIngredientAsync(Models.Groceries.Ingredient ingredient);
    Task<IEnumerable<Models.Groceries.Ingredient>> SearchIngredientsAsync(string query, int limit = 10, CancellationToken cancellationToken = default);

    #endregion
}
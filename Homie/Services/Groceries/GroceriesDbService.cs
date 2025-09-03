using Dapper;
using Homie.Models.Groceries;
using Npgsql;

namespace Homie.Services;

public partial class DbService
{
    public async Task<IEnumerable<Recipe>> GetRecipesAsync()
    {
        string sql = @"SELECT
							r.id AS id,
							r.title AS title,
							i.id AS ingredientid,
							i.title AS ingredienttitle
						FROM
							recipe r
							LEFT OUTER JOIN recipe_ingredient ri ON ri.recipe_id = r.id
							LEFT OUTER JOIN ingredient i ON i.id = ri.ingredient_id
						ORDER BY
							r.title";
        
        await using var connection = new NpgsqlConnection(_connectionString);
        var items = await connection.QueryAsync<Recipe, RecipeIngredient, Recipe>
        (
	        sql,
	        (recipe, recipeIngredient) =>
	        {
		        recipeIngredient.Recipe = recipe;
		        recipe.Ingredients.Add(recipeIngredient);
		        return recipe;
	        },
	        splitOn: "ingredientid"
        );

        var result = items.GroupBy(x => x.Id).Select(x =>
        {
	        var groupedRecipes = x.First();
	        groupedRecipes.Ingredients = x
		        .SelectMany(y => y.Ingredients)
		        .ToList();
	        return groupedRecipes;
        });

        return result;
    }



    public async Task AddRecipeAsync(Recipe recipe, IEnumerable<Ingredient> ingredients)
    {
        // add recipe
        string rSql = @"INSERT INTO
	                            recipe (title)
                            VALUES
	                            (@Title)
	                        RETURNING
	                            id";
        var recipeId = await QueryDbSingleAsync<Guid>(rSql, recipe);

        if (recipeId == Guid.Empty)
        {
            throw new InvalidOperationException("Recipe could not be created");
        }
        
        // update ingredients
        {
            string sql = @"INSERT INTO
	                            ingredient (title)
                            VALUES
	                            (@Title)
                            ON CONFLICT (title) DO nothing";
            var result = await BatchExecuteAsync(sql, ingredients);
        }
        
        // add recipe ingredients
        {
            string sql = @"INSERT INTO
								recipe_ingredient (recipe_id, ingredient_id)
							SELECT
								@RecipeId,
								id
							FROM
								ingredient
							WHERE
								title = @Title
							ON CONFLICT (recipe_id, ingredient_id) DO nothing";
            var result = await BatchExecuteAsync(sql, ingredients, param: new { RecipeId = recipeId });
        }
    }



    public Task UpdateRecipeAsync(Recipe recipe, IEnumerable<Ingredient> ingredients) => ExecuteTransactionAsync(async () =>
    {
        // update recipe
        {
            string sql = @"UPDATE recipe
                            SET
	                            title = @Title
                            WHERE
	                            id = @Id";
            await ExecuteSqlAsync(sql, recipe);
        }
        
        // update ingredients
        {
            string sql = @"INSERT INTO
	                            ingredient (title)
                            VALUES
	                            (@Title)
                            ON CONFLICT (title) DO nothing";
            var result = await BatchExecuteAsync(sql, ingredients);
        }
        
        // update recipe ingredients
        {
            // delete old
            string deleteSql = @"DELETE FROM recipe_ingredient
									WHERE
										ingredient_id != ALL (@Ids)";
            await ExecuteSqlAsync(deleteSql, new { Ids = ingredients.Select(x => x.Id).ToList() });
            
	        // upsert
            string sql = @"INSERT INTO
	                            recipe_ingredient (recipe_id, ingredient_id)
                            SELECT
	                            @RecipeId,
	                            id
                            FROM
	                            ingredient
                            WHERE
	                            title = @Title
	                        ON CONFLICT (recipe_id, ingredient_id) DO nothing";
            var result = await BatchExecuteAsync(sql, ingredients, param: new { RecipeId = recipe.Id });
        }
    });



    public Task DeleteRecipeAsync(Recipe recipe) => ExecuteTransactionAsync(async () =>
    {
        // delete recipe
        {
            string sql = @"DELETE FROM recipe
                            WHERE
	                            id = @Id";
            await ExecuteSqlAsync(sql, recipe);
        }
        
        // delete recipe ingredients
        {
            string sql = @"DELETE FROM recipe_ingredient
                            WHERE
	                            recipe_id = @Id";
            await ExecuteSqlAsync(sql, recipe);
        }
    });



    public async Task<IEnumerable<Ingredient>> GetIngredientsAsync()
    {
        string sql = @"SELECT
	                        id AS id,
	                        title AS title
                        FROM
	                        ingredient";
        return await QueryDbAsync<Ingredient>(sql);
    }



    public async Task<IEnumerable<Ingredient>> GetIngredientsByRecipeIdAsync(Guid recipeId)
    {
	    string sql = @"SELECT
							i.id,
							i.title
						FROM
							ingredient i
							INNER JOIN recipe_ingredient ri ON ri.ingredient_id = i.id
							AND ri.recipe_id = @RecipeId";
	    return await QueryDbAsync<Ingredient>(sql, new { RecipeId = recipeId });
    }



    public async Task AddIngredientAsync(Ingredient ingredient)
    {
        string sql = @"INSERT INTO
	                        ingredient (title)
                        VALUES
	                        (@Title)";
        await ExecuteSqlAsync(sql, ingredient);
    }



    public async Task UpdateIngredientAsync(Ingredient ingredient)
    {
        string sql = @"UPDATE ingredient
                        SET
	                        title = @Title
                        WHERE
	                        id = @Id";
        await ExecuteSqlAsync(sql, ingredient);
    }



    public async Task DeleteIngredientAsync(Ingredient ingredient)
    {
        string sql = @"DELETE FROM ingredient
                        WHERE
	                        id = @Id";
        await ExecuteSqlAsync(sql, ingredient);
    }



    public async Task<IEnumerable<Ingredient>> SearchIngredientsAsync(string query, int limit = 10, CancellationToken cancellationToken = default)
    {
        string sql = @"SELECT
	                        id,
	                        lower(title) as title
                        FROM
	                        ingredient
                        WHERE
                            lower(title) LIKE '%' || @Query || '%'
                        LIMIT
	                        @Limit";
        return await QueryDbAsync<Ingredient>(sql, cancellationToken: cancellationToken, param: new
        {
            Query = query.ToLower(),
            Limit = limit
        });
    }




}
using Dapper;
using Homie.Models.Finances;
using Homie.Types;
using Npgsql;

namespace Homie.Services;

public partial class DbService
{
    public async Task<IEnumerable<FinanceItem>> GetFinancialsAsync(FinanceItemType type)
    {
        string sql = @"SELECT
							t.id AS id,
							t.title AS title,
							t.amount AS amount,
							t.frequency AS frequency,
							t.description AS description,
							t.type AS type,
							t.start_date AS startdate,
							t.end_date AS enddate,
							o.occurrence_date AS occurrencedate,
							o.cum_sum AS cumsum
						FROM
							finance t
							LEFT OUTER JOIN finance_occurrence o ON o.finance_id = t.id
						WHERE
							t.type = @Type
						ORDER BY
						    t.start_date,
						    o.occurrence_date,
							t.amount DESC";

        await using var connection = new NpgsqlConnection(_connectionString);
        var items = await connection.QueryAsync<FinanceItem, FinanceOccurrence, FinanceItem>
        (
	        sql, 
	        (financeItem, occurrence) =>
	        {
		        occurrence.ParentItem = financeItem;
		        financeItem.TimeSeries.Add(occurrence);
		        return financeItem;
	        }, 
	        splitOn: "occurrencedate", 
	        param: type
	        );

        var result = items.GroupBy(x => x.Id).Select(x =>
        {
	        var groupedFinances = x.First();
	        groupedFinances.TimeSeries = x
		        .SelectMany(y => y.TimeSeries)
		        .ToList();
	        return groupedFinances;
        });

        return result;
    }

    public async Task AddFinanceItemAsync(FinanceItem financeItem) => await ExecuteTransactionAsync(async () =>
    {
	    // Create finance row
	    string sql = @"INSERT INTO
							finance (title, amount, frequency, description, type, start_date, end_date)
						VALUES
							(@Title, @Amount, @Frequency, @Description, @Type, @StartDate, @EndDate)
						RETURNING id";
	    var newId = await QueryDbSingleAsync<Guid>(sql, financeItem);

	    // Create time series
	    await UpsertFinancialOccurrencesAsync(newId);
    });


    public async Task UpdateFinanceItemAsync(FinanceItem financeItem) => await ExecuteTransactionAsync(async () =>
    {
	    // Update finance row
	    string sql = @"UPDATE
							finance
						SET
							title = @Title,
							amount = @Amount,
							frequency = @Frequency,
							description = @Description,
							type = @Type,
							start_date = @StartDate,
							end_date = @EndDate
						WHERE
						    id = @Id";
	    var rowsAffected = await ExecuteSqlAsync(sql, financeItem);
	    
	    if (rowsAffected == 0) 
		    return;
	    
	    // Update time series
	    await UpsertFinancialOccurrencesAsync(financeItem.Id);
    });


    public async Task DeleteFinanceItemAsync(Guid financeId) => await ExecuteTransactionAsync(async () => 
    {
	    // Delete finance row
	    {
		    string sql = @"DELETE FROM finance WHERE id = @FinanceId";
		    await ExecuteSqlAsync(sql, new { FinanceId = financeId });
	    }
	    
	    // Delete finance time series
	    {
		    string sql = @"DELETE FROM finance_occurrence WHERE finance_id = @FinanceId";
		    await ExecuteSqlAsync(sql, new { FinanceId = financeId });
	    }
    });

    

    public async Task UpsertFinancialOccurrencesAsync(Guid financeId)
    {
	    // Delete (old) finance time series
	    {
		    string sql = @"DELETE FROM finance_occurrence WHERE finance_id = @FinanceId";
		    await ExecuteSqlAsync(sql, new { FinanceId = financeId });
	    }
	    
	    // Create finance time series
	    {
		    string sql = @"INSERT INTO
							finance_occurrence (finance_id, occurrence_date, cum_sum)
						SELECT
							t.id,
							s.dt,
							sum(t.amount) OVER (
								PARTITION BY
									t.id
								ORDER BY
									dt
							) AS cum_sum
						FROM
							finance t,
							generate_series(
								t.start_date,
								t.end_date,
								CASE
									WHEN t.frequency = 'Once' THEN INTERVAL '1000000' DAY
									WHEN t.frequency = 'Daily' THEN INTERVAL '1' DAY
									WHEN t.frequency = 'Weekly' THEN INTERVAL '7' DAY
									WHEN t.frequency = 'Bi-Weekly' THEN INTERVAL '14' DAY
									WHEN t.frequency = 'Monthly' THEN INTERVAL '1' MONTH
									ELSE INTERVAL '1000000' DAY
								END
							) AS s (dt)
						WHERE
							id = @FinanceId";
		    await ExecuteSqlAsync(sql, new { FinanceId = financeId });
	    }
	}
}
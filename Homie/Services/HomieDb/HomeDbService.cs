using Homie.HomeDb.Models.Bank;
using Homie.Interfaces;
using Homie.Services;

namespace Homie.HomeDb;

public partial class HomieDbService : DapperBase, IHomieDbService
{
    public HomieDbService(string connectionString) : base(connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));
    }


    public async Task<IEnumerable<BankCharge>> GetBankChargesAsync(DateTime dateStart, DateTime dateEnd)
    {
        string sql = @"SELECT
							id,
							amount,
							value,
							payee,
							email_date::TIMESTAMPTZ AS emaildate,
							sum(value) OVER (
								PARTITION BY
									payee
							) AS payeetotal,
							sum(value) OVER () AS total
						FROM
							n8n.bank_charge
						WHERE
							found = TRUE
							AND email_date::TIMESTAMPTZ BETWEEN @DateStart AND @DateEnd
						ORDER BY
							value DESC,
							payee";

        return await QueryDbAsync<BankCharge>(sql, new
        {
            DateStart = dateStart,
            DateEnd = dateEnd,
        });
    }
}
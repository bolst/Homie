using Dapper;
using Homie.Interfaces;
using Homie.Models;
using Homie.Types;
using Npgsql;

namespace Homie.Services;

public partial class DbService : DapperBase, IDbService
{
	public DbService(string connectionString) : base(connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
			throw new ArgumentNullException(nameof(connectionString));
	}
}
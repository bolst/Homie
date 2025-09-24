namespace Homie.Interfaces;

using Db.Models.Bank;

public interface IHomieDbService
{
    Task<IEnumerable<BankCharge>> GetBankChargesAsync(DateTime dateStart, DateTime dateEnd);
}
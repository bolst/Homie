namespace Homie.Interfaces;

using HomeDb.Models.Bank;

public interface IHomeDbService
{
    Task<IEnumerable<BankCharge>> GetBankChargesAsync(DateTime dateStart, DateTime dateEnd);
}
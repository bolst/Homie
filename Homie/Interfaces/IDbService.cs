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
}
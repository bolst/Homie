using Homie.Models.Jobs;

namespace Homie.Services;

public partial class DbService
{
    public async Task<IEnumerable<JobApplication>> GetJobApplicationsAsync()
    {
        string sql = @"SELECT
							id AS id,
							POSITION AS POSITION,
							company AS company,
							industry AS industry,
							location AS location,
							date_posted AS dateposted,
							date_applied AS dateapplied,
							notes AS notes,
							link AS link,
							status AS status
						FROM
							job_application";
        return await QueryDbAsync<JobApplication>(sql);
    }



    public async Task AddJobApplicationAsync(JobApplication jobApplication)
    {
	    string sql = @"INSERT INTO
							job_application (
								position,
								company,
								industry,
								location,
								date_posted,
								date_applied,
								notes,
								link,
							    status
							)
						VALUES
							(@Position, @Company, @Industry, @Location, @DatePosted, @DateApplied, @Notes, @Link, @Status)";
	    
	    await ExecuteSqlAsync(sql, jobApplication);
    }
    
    
    
    public async Task UpdateJobApplicationAsync(JobApplication jobApplication)
	{
		string sql = @"UPDATE job_application
						SET
							position = @Position,
							company = @Company,
							industry = @Industry,
							location = @Location,
							date_posted = @DatePosted,
							date_applied = @DateApplied,
							notes = @Notes,
							link = @Link,
							status = @Status
						WHERE
							id = @Id";
		await ExecuteSqlAsync(sql, jobApplication);
	}
    
    
    
    public async Task DeleteJobApplicationAsync(Guid jobApplicationId)
	{
		string sql = @"DELETE FROM job_application
						WHERE
							id = @Id";
		await ExecuteSqlAsync(sql, new { Id = jobApplicationId });
	}
}
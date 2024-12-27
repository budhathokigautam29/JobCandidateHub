using JobCandidateHubAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobCandidateHubAPI.ViewModels.JobCandidate;
using Dapper;
using System.Data;

namespace JobCandidateHubAPI.Services
{
    public class JobCandidateService : IJobCandidate
    {
        private IDatabaseConnectionFactory _db;
        public JobCandidateService(
           IDatabaseConnectionFactory db)
        {
            _db = db;
        }

        public async Task<ICollection<JobCandidateViewModel>> GetAllAsync()
        {
            StringBuilder strSql = new StringBuilder(@"");
            try
            {
                using var con = await _db.CreateConnectionAsync();

                var jobCandidate = await con.QueryAsync<JobCandidateViewModel>(strSql.ToString());
                return jobCandidate.AsList();
            }
            catch (Exception ex)
            {
                return new List<JobCandidateViewModel>();
            }
        }
        public async Task<JobCandidateViewModel> GetByIdAsync(int id)
        {
            string strSql = @"";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", id, DbType.Int32);

            try
            {
                using var con = await _db.CreateConnectionAsync();

                JobCandidateViewModel jobCandidate = await con.QueryFirstAsync<JobCandidateViewModel>(strSql, parameters);

                return jobCandidate;
            }
            catch (Exception ex)
            {
                return new JobCandidateViewModel();
            }
        }
        public async Task<int> CreateAsync(JobCandidateViewModel jobCandidateVM)
        {

            var sqlQuery = @"
					SELECT
						SCOPE_IDENTITY()";

            jobCandidateVM.CreatedTs = DateTime.UtcNow;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("FirstName", jobCandidateVM.FirstName, DbType.String);
            parameters.Add("LastName", jobCandidateVM.LastName, DbType.String);
            parameters.Add("Email", jobCandidateVM.Email, DbType.String);
            parameters.Add("TimeInterval", jobCandidateVM.TimeInterval, DbType.String);
            parameters.Add("CreatedBy", jobCandidateVM.CreatedBy, DbType.String);
            parameters.Add("PhoneNumber", jobCandidateVM.PhoneNumber, DbType.Double);
            parameters.Add("CreatedBy", jobCandidateVM.CreatedBy, DbType.Int32);
            parameters.Add("UpdatedBy", jobCandidateVM.UpdatedBy, DbType.Int32);
            parameters.Add("CreatedTs", jobCandidateVM.CreatedTs, DbType.DateTime);
            parameters.Add("UpdatedTs", jobCandidateVM.UpdatedTs, DbType.DateTime);

            try
            {
                using var con = await _db.CreateConnectionAsync();
                int jobCandidateId = Convert.ToInt32(await con.ExecuteScalarAsync(sqlQuery, parameters));
                return jobCandidateId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<int> UpdateAsync(int jobCandidateId, JobCandidateViewModel jobCandidateViewModel)
        {
            string strSql = @"
                    IF EXISTS (SELECT AgentId 
                    FROM Agent
                    WHERE [Email] = @Email
		                    AND AgentId != @AgentId)
                    BEGIN
	                    SELECT -1
                    END
                    ELSE BEGIN
	                    
                        SELECT @AgentId
                    END";

            jobCandidateViewModel.UpdatedTs = DateTime.UtcNow;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("FirstName", jobCandidateViewModel.FirstName, DbType.String);
            parameters.Add("LastName", jobCandidateViewModel.LastName, DbType.String);
            parameters.Add("Email", jobCandidateViewModel.Email, DbType.String);
            parameters.Add("TimeInterval", jobCandidateViewModel.TimeInterval, DbType.String);
            parameters.Add("CreatedBy", jobCandidateViewModel.CreatedBy, DbType.String);
            parameters.Add("PhoneNumber", jobCandidateViewModel.PhoneNumber, DbType.Double);
            parameters.Add("CreatedBy", jobCandidateViewModel.CreatedBy, DbType.Int32);
            parameters.Add("UpdatedBy", jobCandidateViewModel.UpdatedBy, DbType.Int32);
            parameters.Add("CreatedTs", jobCandidateViewModel.CreatedTs, DbType.DateTime);
            parameters.Add("UpdatedTs", jobCandidateViewModel.UpdatedTs, DbType.DateTime);
            try
            {
                using var con = await _db.CreateConnectionAsync();
                return await con.ExecuteScalarAsync<int>(strSql, parameters);
            }
            catch (Exception ex)
            {                
                return 0;
            }
        }
    }
}

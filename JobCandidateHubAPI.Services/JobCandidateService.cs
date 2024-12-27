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
            string strSql = @"SELECT 
                             JobCandidateId
                             , FirstName
                             , LastName
                             , Email
                             , PhoneNumber
                             , TimeInterval
                             , LinkedInUrl
                             , GitHubUrl
                             , Comment
                             , CreatedBy
                             , UpdatedBy
                             , CreatedTs
                             , UpdatedTs
                             FROM JobCandidate";
            try
            {
                using var con = await _db.CreateConnectionAsync();

                var jobCandidate = await con.QueryAsync<JobCandidateViewModel>(strSql);
                return jobCandidate.AsList();
            }
            catch (Exception ex)
            {
                return new List<JobCandidateViewModel>();
            }
        }
        public async Task<JobCandidateViewModel> GetByIdAsync(int id)
        {
            string strSql = @"SELECT 
                              JobCandidateId
                              , FirstName
                              , LastName
                              , Email
                              , PhoneNumber
                              , TimeInterval
                              , LinkedInUrl
                              , GitHubUrl
                              , Comment
                              , CreatedBy
                              , UpdatedBy
                              , CreatedTs
                              , UpdatedTs
                              FROM JobCandidate
                              WHERE JobCandidateId = @JobCandidateId";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("JobCandidateId", id, DbType.Int32);

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

            var sqlQuery = @"INSERT INTO JobCandidate 
                            (
                             FirstName
                            , LastName
                            , Email
                            , PhoneNumber
                            , TimeInterval
                            , LinkedInUrl
                            , GitHubUrl
                            , Comment
                            , CreatedBy
                            , UpdatedBy
                            , CreatedTs
                            , UpdatedTs
                            )
                            VALUES
                            (
                             @FirstName
                            , @LastName
                            , @Email
                            , @PhoneNumber
                            , @TimeInterval
                            , @LinkedInUrl
                            , @GitHubUrl
                            , @Comment
                            , @CreatedBy
                            , @UpdatedBy
                            , @CreatedTs
                            , @UpdatedTs
                            )
					        SELECT
					        	SCOPE_IDENTITY()";

            jobCandidateVM.CreatedTs = DateTime.UtcNow;
            jobCandidateVM.CreatedBy = 1; // since we dont have users for now hardcoded value : 1
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@FirstName", jobCandidateVM.FirstName, DbType.String);
            parameters.Add("@LastName", jobCandidateVM.LastName, DbType.String);
            parameters.Add("@Email", jobCandidateVM.Email, DbType.String);
            parameters.Add("@PhoneNumber", jobCandidateVM.PhoneNumber, DbType.String);
            parameters.Add("@TimeInterval", jobCandidateVM.TimeInterval, DbType.String);
            parameters.Add("@LinkedInUrl", jobCandidateVM.LinkedInUrl, DbType.String);
            parameters.Add("@GitHubUrl", jobCandidateVM.GitHubUrl, DbType.String);
            parameters.Add("@Comment", jobCandidateVM.Comment, DbType.String);
            parameters.Add("@CreatedBy", jobCandidateVM.CreatedBy, DbType.String);
            parameters.Add("@UpdatedBy", jobCandidateVM.UpdatedBy, DbType.Int32);
            parameters.Add("@CreatedTs", jobCandidateVM.CreatedTs, DbType.DateTime);
            parameters.Add("@UpdatedTs", jobCandidateVM.UpdatedTs, DbType.DateTime);

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
                    IF EXISTS (SELECT JobCandidateId 
                    FROM JobCandidate
                    WHERE [Email] = @Email
		                    AND JobCandidateId != @JobCandidateId)
                    BEGIN
	                    SELECT -1
                    END
                    ELSE BEGIN
	                    UPDATE JobCandidate
                        SET 
                         FirstName			=@FirstName
                        , LastName			= @LastName
                        , Email				= @Email
                        , PhoneNumber		= @PhoneNumber
                        , TimeInterval		= @TimeInterval
                        , LinkedInUrl		= @LinkedInUrl
                        , GitHubUrl			= @GitHubUrl
                        , Comment           = @Comment
                        , CreatedBy			= @CreatedBy
                        , UpdatedBy			= @UpdatedBy
                        , CreatedTs			= @CreatedTs
                        , UpdatedTs			= @UpdatedTs
                        WHERE JobCandidateId = @JobCandidateId
                        SELECT @JobCandidateId
                    END";

            jobCandidateViewModel.UpdatedTs = DateTime.UtcNow;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@JobCandidateId", jobCandidateId, DbType.Int32);
            parameters.Add("@FirstName", jobCandidateViewModel.FirstName, DbType.String);
            parameters.Add("@LastName", jobCandidateViewModel.LastName, DbType.String);
            parameters.Add("@Email", jobCandidateViewModel.Email, DbType.String);
            parameters.Add("@PhoneNumber", jobCandidateViewModel.PhoneNumber, DbType.String);
            parameters.Add("@TimeInterval", jobCandidateViewModel.TimeInterval, DbType.String);
            parameters.Add("@LinkedInUrl", jobCandidateViewModel.LinkedInUrl, DbType.String);
            parameters.Add("@GitHubUrl", jobCandidateViewModel.GitHubUrl, DbType.String);
            parameters.Add("@Comment", jobCandidateViewModel.Comment, DbType.String);
            parameters.Add("@CreatedBy", jobCandidateViewModel.CreatedBy, DbType.String);
            parameters.Add("@UpdatedBy", jobCandidateViewModel.UpdatedBy, DbType.Int32);
            parameters.Add("@CreatedTs", jobCandidateViewModel.CreatedTs, DbType.DateTime);
            parameters.Add("@UpdatedTs", jobCandidateViewModel.UpdatedTs, DbType.DateTime);
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
        public async Task<int> DeleteAsync(int jobCandidateId)
        {
            string strSql = @"                    
	                    DELETE FROM JobCandidate
                        WHERE JobCandidateId = @JobCandidateId
                        SELECT @JobCandidateId";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@JobCandidateId", jobCandidateId, DbType.Int32);
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

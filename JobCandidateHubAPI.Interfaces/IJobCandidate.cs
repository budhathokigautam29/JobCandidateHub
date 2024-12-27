using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobCandidateHubAPI.ViewModels.JobCandidate;

namespace JobCandidateHubAPI.Interfaces
{
    public interface IJobCandidate
    {
        Task<ICollection<JobCandidateViewModel>> GetAllAsync();
        Task<JobCandidateViewModel> GetByIdAsync(int id);
        Task<int> CreateAsync(JobCandidateViewModel jobCandidateViewModel);
        Task<int> UpdateAsync(int jobCandidateId, JobCandidateViewModel jobCandidateViewModel);
        Task<int> DeleteAsync(int id);
    }
}

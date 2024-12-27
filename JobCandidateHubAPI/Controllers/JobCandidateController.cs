using JobCandidateHubAPI.Interfaces;
using JobCandidateHubAPI.Models.Entity;
using JobCandidateHubAPI.ViewModels.JobCandidate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobCandidateHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCandidateController : ControllerBase
    {
        public readonly IJobCandidate _jobCandidateService;
        public JobCandidateController(IJobCandidate jobCandidateService)
        {
            _jobCandidateService = jobCandidateService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            ICollection<JobCandidateViewModel> jobCandidateVM = await _jobCandidateService.GetAllAsync();
            if (jobCandidateVM == null)
                return Ok(new { success = false, message = "No job candidate found" });
            return Ok(new { success = true, jobCandidateVM });

        }
        
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            JobCandidateViewModel jobCandidateVM = await _jobCandidateService.GetByIdAsync(id);
            if (jobCandidateVM == null)
                return Ok(new { success = false, message = "No job candidate found" });
            return Ok(new { success = true, jobCandidateVM });
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(JobCandidateViewModel jobCandidateVM)
        {
            int jobCandidateId = await _jobCandidateService.CreateAsync(jobCandidateVM);
            if (jobCandidateId > 0)
                return Ok(new { success = true, message = "Job Candidate created successfully", jobCandidateId });
            if (jobCandidateId == -1)
                return Ok(new { success = false, message = "Job Candidate with this name already exists" });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { success = false, message = "Unable to create Job Candidate" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, JobCandidateViewModel jobCandidateVM)
        {
            int result = await _jobCandidateService.UpdateAsync(id, jobCandidateVM);
            if (result > 0)
                return Ok(new { success = true, message = "Job Candidate edited successfully" });            
            else
                return StatusCode(StatusCodes.Status404NotFound, new { success = false, message = "Unable to edit Job Candidate" });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            int result = await _jobCandidateService.DeleteAsync(id);
            if (result > 0)
                return Ok(new { success = true, message = "Job Candidate deleted successfully" });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { success = false, message = "Unable to delete Job Candidate" });
        }
    }
}

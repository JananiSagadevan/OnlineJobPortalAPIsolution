using Jobportal.BusinessService;
using Jobportal.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineJobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecruiterController : ControllerBase
    {
        private readonly IRecruiterBusinessService _recruiterBusinessService;

        public RecruiterController(IRecruiterBusinessService recruiterBusinessService)
        {
            _recruiterBusinessService = recruiterBusinessService;
        }

        [HttpPost("createprofile")]
        public IActionResult CreateProfile(RecruiterProfile profile)
        {
            try
            {
                bool result = _recruiterBusinessService.CreateRecruiterProfile(profile);
                return result ? Ok("Profile created") : BadRequest("Failed to create profile");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("getprofile")]
        public IActionResult GetProfile(int recruiterId)
        {
            try
            {
                var profile = _recruiterBusinessService.GetRecruiterProfile(recruiterId);
                return profile != null ? Ok(profile) : NotFound("Profile not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("updateprofile")]
        public IActionResult UpdateProfile(RecruiterProfile profile)
        {
            try
            {
                bool result = _recruiterBusinessService.UpdateRecruiterProfile(profile);
                return result ? Ok(new { message="Profile updated" }) : BadRequest(new {message = "Update failed"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("addjob")]
        public IActionResult AddJob(Job job)
        {
            try
            {
                var result = _recruiterBusinessService.AddJob(job);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("deletejob")]
        public IActionResult DeleteJob(int jobId)
        {
            try
            {
                bool result = _recruiterBusinessService.DeleteJob(jobId);
                return result ? Ok(new { message = "Deleted" }) : NotFound(new { message = "Job not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("getjobbyrecruiter")]
        public IActionResult GetJobsByRecruiterId(int recruiterId)
        {
            try
            {
                var jobs = _recruiterBusinessService.GetJobsByRecruiterId(recruiterId);
                return Ok(new
                {
                    status = "success",
                    message = "Jobs retrieved successfully",
                    data = jobs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }

        }

        [HttpGet("GetApplicationsByJobSeekerbyid")]
        public IActionResult GetApplicationsByJobSeeker(int jobSeekerId)
        {
            try
            {
                var applications = _recruiterBusinessService.GetApplicationsByJobSeeker(jobSeekerId);
                return Ok(applications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetApplicationByapplicationId")]
        public IActionResult GetApplicationById(int applicationId)
        {
            try
            {
                var application = _recruiterBusinessService.GetApplicationById(applicationId);
                return application != null ? Ok(application) : NotFound("Application not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("add-status")]
        public IActionResult AddStatus(ApplicationStatus status)
        {
            try
            {
                var result = _recruiterBusinessService.AddStatus(status);
                return result ? Ok("Status added successfully") : BadRequest("Failed to add status");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-status")]
        public IActionResult UpdateStatus(ApplicationStatus status)
        {
            try
            {
                var result = _recruiterBusinessService.UpdateStatus(status);
                return result ? Ok("Status updated successfully") : BadRequest("Failed to update status");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}


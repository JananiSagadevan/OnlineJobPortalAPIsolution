using Jobportal.BusinessService;
using Jobportal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserBusinessService _userBusinessService;

    public UserController(IUserBusinessService userBusinessService)
    {
        _userBusinessService = userBusinessService;
    }

    [HttpPost("createprofile")]
    public IActionResult CreateProfile([FromBody] JobSeekerProfile profile)
    {
        try
        {
            var result = _userBusinessService.CreateProfile(profile);
            return Ok(result);
        }
        catch (Exception ex)
        {
            
            return StatusCode((int)HttpStatusCode.InternalServerError, new
            {
                StatusCode = -1,
                Message = $"Error while creating profile: {ex.Message}"
            });
        }
    }

    [HttpGet("GetProfile")]
    public IActionResult GetProfile(int jobSeekerId)
    {
        try
        {
            var (profile, statusCode, message) = _userBusinessService.JobSeekerProfiles(jobSeekerId);
            if (statusCode == -1)
            {
                return NotFound(new { StatusCode = statusCode, Message = message });
            }
            return Ok(new { StatusCode = statusCode, Message = message, Profile = profile });
        }
        catch (Exception ex)
        { 
            return StatusCode((int)HttpStatusCode.InternalServerError, new
            {
                StatusCode = -1,
                Message = $"Error while retrieving profile: {ex.Message}"
            });
        }
    }

    [HttpPut("UpdateProfile")]
    public IActionResult UpdateProfile(int jobSeekerId, [FromBody] JobSeekerProfile profile)
    {
        try
        {
            var (isSuccess, statusCode, message) = _userBusinessService.UpdateProfile(jobSeekerId, profile);
            if (statusCode == -1)
            {
                return BadRequest(new { StatusCode = statusCode, Message = message });
            }
            return Ok(new { IsSuccess = isSuccess, StatusCode = statusCode, Message = message });
        }
        catch (Exception ex)
        {  
            return StatusCode((int)HttpStatusCode.InternalServerError, new
            {
                StatusCode = -1,
                Message = $"Error while updating profile: {ex.Message}"
            });
        }
    }

    [HttpGet("GetAllJobs")]
    public IActionResult GetAllJobs()
    {
        try
        {
            var (jobs, statusCode, message) = _userBusinessService.GetAllJobs();
            if (statusCode == -1)
            {
                return NotFound(new { StatusCode = statusCode, Message = message });
            }
            return Ok(new { StatusCode = statusCode, Message = message, Jobs = jobs });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new
            {
                StatusCode = -1,
                Message = $"Error while fetching jobs: {ex.Message}"
            });
        }
    }

    [HttpGet("GetJobById")]
    public IActionResult GetJobById(int jobId)
    {
        try
        {
            var (job, statusCode, message) = _userBusinessService.GetJobById(jobId);
            if (statusCode == -1)
            {
                return NotFound(new { StatusCode = statusCode, Message = message });
            }
            return Ok(new { StatusCode = statusCode, Message = message, Job = job });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new
            {
                StatusCode = -1,
                Message = $"Error while fetching job details: {ex.Message}"
            });
        }
    }

    [HttpPost("applyjobs")]
    public IActionResult SubmitApplication([FromBody] JobApplication application)
    {
        try
        {
            var (isSuccess, statusCode, message) = _userBusinessService.SubmitApplication(application);
            if (statusCode == -1)
            {
                return BadRequest(new { StatusCode = statusCode, Message = message });
            }
            return Ok(new { IsSuccess = isSuccess, StatusCode = statusCode, Message = message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new
            {
                StatusCode = -1,
                Message = $"Error while submitting application: {ex.Message}"
            });
        }
    }

    [HttpGet("GetStatusHistory")]
    public IActionResult GetStatusHistory(int applicationId)
    {
        try
        {
            var (history, isSuccess, statusCode, message) = _userBusinessService.GetStatusHistory(applicationId);
            if (statusCode == -1)
            {
                return NotFound(new { StatusCode = statusCode, Message = message });
            }
            return Ok(new { IsSuccess = isSuccess, StatusCode = statusCode, Message = message, History = history });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new
            {
                StatusCode = -1,
                Message = $"Error while fetching application status: {ex.Message}"
            });
        }
    }
}

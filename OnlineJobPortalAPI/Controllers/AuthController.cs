using Microsoft.AspNetCore.Mvc;
using Jobportal.BusinessService;
using Jobportal.Models;
using System;

namespace OnlineJobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthBusinessService _AuthBusinessService;

        public AuthController(IAuthBusinessService AuthBusinessService)
        {
            _AuthBusinessService = AuthBusinessService;
        }

        [HttpPost("register/user")]
        public IActionResult RegisterUser(JobSeeker jobSeeker)
        {
            try
            {
                int result = _AuthBusinessService.UserRegister(jobSeeker);
                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        /*[HttpPost("login/user")]
        public IActionResult UserLogin([FromBody] LoginRequest request)
        {
            try
            {
                var user = _AuthBusinessService.UserLogin(request.Email, request.Password);
                return user != null ? Ok(user) : Unauthorized("Invalid email or password");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }*/
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            int errorCode;
            string errorMessage;

            var user = _AuthBusinessService.Login(request.Email, request.Password, out errorCode, out errorMessage);

            if (errorCode == 1)
                return Ok(new { status = "success", message = errorMessage, data = user });
            else
                return BadRequest(new { status = "error", message = errorMessage });
        }

        [HttpPost("register/recruiter")]
        public IActionResult RegisterRecruiter(Recruiter recruiter)
        {
            try
            {
                int result = _AuthBusinessService.RecruiterRegister(recruiter);
                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("login/recruiter")]
        public IActionResult RecruiterLogin([FromBody] RecruiterLoginRequest login)
        {
            try
            {
                var (recruiter, statusCode, message) = _AuthBusinessService.RecruiterLogin(login.Email, login.Password);

                if (recruiter == null)
                {
                    return StatusCode(statusCode, new { Success = false, Message = message });
                }

                return Ok(new { Success = true, Data = recruiter });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

    }
}

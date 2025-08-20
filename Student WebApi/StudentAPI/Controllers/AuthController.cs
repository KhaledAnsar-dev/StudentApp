using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_API_Project.Models;
using Student_API_Project.Services.Auth;
using StudentBusiness;
using StudentData.DataModels;

namespace Student_API_Project.Controllers
{
    // This controller handles authentication requests
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Dependency injection for JWT token generator
        private readonly IJwtTokenGenerator _jwt;

        // Constructor that initializes the JWT generator
        public AuthController(IJwtTokenGenerator jwt) => _jwt = jwt;

        // Endpoint: POST api/Auth/Login
        // Possible responses: 200 OK, 400 Bad Request, 401 Unauthorized
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            // Validate the user credentials (email and password)
            var userDto = AuthService.ValidateUser(loginDto.Email, loginDto.Password);

            // If user is not valid, return 401 Unauthorized
            if (userDto == null)
                return Unauthorized();

            // If valid, generate a JWT token for the user
            var token = _jwt.Generate(userDto);

            // Return the token in the response body
            return Ok(new { token });
        }
    }
}

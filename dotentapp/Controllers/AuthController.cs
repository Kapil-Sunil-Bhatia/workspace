using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using dotnetapp.Models;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto model)
        {
            var user = new ApplicationUser 
            { 
                UserName = model.Email, 
                Email = model.Email,
                IsAdmin = model.IsAdmin 
            };
            
            var result = _userManager.CreateAsync(user, model.Password).Result;
            
            if (result.Succeeded)
            {
                return Ok(new { message = "Registration successful" });
            }
            
            return BadRequest(result.Errors);
        }
        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            var result = _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false).Result;
            
            if (result.Succeeded)
            {
                var user = _userManager.FindByEmailAsync(model.Email).Result;
                return Ok(new { 
                    message = "Login successful",
                    userId = user.Id,
                    isAdmin = user.IsAdmin 
                });
            }
            
            return BadRequest(new { message = "Invalid credentials" });
        }
        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return Ok(new { message = "Logout successful" });
        }
        
        [HttpGet("me")]
        public IActionResult Me()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.GetUserAsync(User).Result;
                return Ok(new { 
                    userId = user.Id, 
                    email = user.Email, 
                    isAdmin = user.IsAdmin 
                });
            }
            
            return Unauthorized();
        }
    }
    
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
    
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

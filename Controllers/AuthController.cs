using FastLead.Models;
using FastLead.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FastLead.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private AuthService _authService;
        public AuthController(AuthService service) { 
            _authService = service;
        }

        [AllowAnonymous]
        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [AllowAnonymous]
        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] User loginData)
        {
            try
            {
                string token = await _authService.Login(loginData.Name, loginData.Password);
                Response.Cookies.Append("jwt-token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(3)
                });
                return Ok(new { message = "Вы вошли в систему", token = token});
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(new {message = "Пользователь не найден либо пароль неверный"});
            }
        }
        [AllowAnonymous]
        [HttpPost("/auth/register")]
        public async Task<IActionResult> Register([FromBody] User registerData)
        {
            bool res = await _authService.Register(registerData.Name, registerData.Password);
            if (!res)
            {
                return BadRequest(new { message = "Такой пользователь уже существует" });
            }
            return Ok(new { message = "Вы успешно зарегестрированы" });
        }
    }
}

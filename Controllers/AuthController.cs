using Microsoft.AspNetCore.Mvc;
using FastLead.Models;
using FastLead.Services;

namespace FastLead.Controllers
{
    public class AuthController : Controller
    {
        private AuthService _authService;
        public AuthController(AuthService service) { 
            _authService = service;
        }

        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View("Login");
        }


        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] User loginData)
        {
            try
            {
                string token = await _authService.Login(loginData.Name, loginData.Password);
                return Ok(new { message = "Вы вошли в систему", token = token});
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(new {message = "Пользователь не найден либо пароль неверный"});
            }
        }

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

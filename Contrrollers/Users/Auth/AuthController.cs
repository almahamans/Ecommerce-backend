using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("/api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            var result = await _authService.RegisterUserService(userRegisterDto);
            return Created("", result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var token = await _authService.LoginService(userLoginDto);
            return Created("", new { Token = token });
        }
    }

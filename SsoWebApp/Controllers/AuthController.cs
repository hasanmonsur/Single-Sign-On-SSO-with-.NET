using Microsoft.AspNetCore.Mvc;
using SsoWebApp.Contacts;
using SsoWebApp.Models;
using SsoWebApp.Services;
using System.Text.Json;
using System;

namespace SsoWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var tokens = HttpContext.Session.GetString("token");
                // Deserialize JSON string into a Person object
                var tokenData = JsonSerializer.Deserialize<AuthModels>(tokens);

                await _authService.RegisterAsync(model, tokenData.token);
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _authService.LoginAsync(model);
                // Store the token in session or local storage as required
                // Store data in the session
                HttpContext.Session.SetString("token", token);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}

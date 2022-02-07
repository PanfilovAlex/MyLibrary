using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.Data.Models;


namespace WebApiMyLib.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private string generetedToken = null;
        public LoginController(IConfiguration configuration, ITokenService token, IUserService user)
        {
            _userService = user;
            _tokenService = token;
            _config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public IActionResult Login(UserModel user)
        {
            if(string.IsNullOrEmpty(user.UserName)||
                string.IsNullOrEmpty(user.Password))
            {
                return BadRequest();
            }

            IActionResult result = Unauthorized();
            var vadidUser = _userService.GetUser(user);

            if(vadidUser != null)
            {
                generetedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(),
                    _config["Jwt:Issuer"].ToString(), vadidUser);
                if(generetedToken != null)
                {
                    HttpContext.Session.SetString("Token", generetedToken);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }

        }
    }
}

using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Logar([FromBody] Usuario model)
        {
            if(string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("Insira um email");
            }
            if(string.IsNullOrEmpty(model.Senha))
            {
                return BadRequest("Insira a senha");
            }
            model.Senha = Services.Servicos.EncriptarSenhas(model.Senha);
            var usuario =  _context.Usuarios.Where(x => x.Email.Equals(model.Email) && x.Senha.Equals(model.Senha)).FirstOrDefault();

            if(usuario != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, model.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ChaveSecreta"]));

                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(8),
                    signingCredentials: cred
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            return BadRequest("Credenciais Inválidas");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AZTU_Akademik.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AZTU_Akademik.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : Controller
    {

        private IConfiguration _config;

        public SignInController(IConfiguration config)
        {
            _config = config;
        }

        //POST
        [HttpPost]
        public async Task<JsonResult> Post(LoginResearcher _user)
        {
            AztuAkademikContext aztuAkademik = new AztuAkademikContext();

            if (!_user.ArasdirmaciEmeil.Contains("@aztu.edu.az"))
                return Json(new { res = 0 });

            var valid_user = aztuAkademik.Arasdirmacilar.Include(x => x.Rol).FirstOrDefault(x => x.ArasdirmaciEmeil == _user.ArasdirmaciEmeil && x.ArasdirmaciPassword == _user.ArasdirmaciPassword);


            if (valid_user != null)
            {
                //var tokenStr = GenerateJSONWebToken(valid_user);

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, valid_user.ArasdirmaciEmeil));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, valid_user.Id.ToString()));
                claimsIdentity.AddClaim(new Claim("FullName", valid_user.ArasdirmaciAd + " " + valid_user.ArasdirmaciSoyad));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, valid_user.Rol.RolAd));

                AuthenticationProperties authProperty = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddYears(1),
                    IssuedUtc = DateTime.UtcNow
                };

                var principial = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principial, authProperty);

                return Json(1);
            }

            return Json(0);

        }

        private string GenerateJSONWebToken(Arasdirmacilar user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.ArasdirmaciEmeil),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.ArasdirmaciAd + " " + user.ArasdirmaciSoyad),
                new Claim(JwtRegisteredClaimNames.Typ,user.Rol.RolAd),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddYears(1),
                signingCredentials: credentials
            );

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodetoken;

        }
    }
}
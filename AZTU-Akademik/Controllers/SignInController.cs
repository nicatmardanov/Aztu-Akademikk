using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : Controller
    {

        //POST
        [HttpPost]
        public async Task<JsonResult> Post(User _user)
        {
            AztuAkademikContext aztuAkademik = new AztuAkademikContext();

            if (!_user.Email.Contains("@aztu.edu.az"))
                return Json(new { res = 0 });

            var valid_user = aztuAkademik.User.FirstOrDefault(x => x.Email == _user.Email && x.Password == _user.Password);


            if (valid_user != null)
            {
                //var tokenStr = GenerateJSONWebToken(valid_user);

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, valid_user.Email));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, valid_user.Id.ToString()));
                claimsIdentity.AddClaim(new Claim("FullName", valid_user.FirstName + " " + valid_user.LastName));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, valid_user.RoleId.ToString()));

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

    }
}
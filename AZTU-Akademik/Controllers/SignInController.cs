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

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : Controller
    {
        //POST
        [HttpPost]
        public async Task<JsonResult> Post(LoginResearcher _user)
        {
            AztuAkademikContext aztuAkademik = new AztuAkademikContext();

            if (!_user.ArasdirmaciEmeil.Contains("@aztu.edu.az"))
                return Json(new { res = 0 });

            var valid_user = aztuAkademik.Arasdirmacilar.FirstOrDefault(x => x.ArasdirmaciEmeil == _user.ArasdirmaciEmeil && x.ArasdirmaciPassword == _user.ArasdirmaciPassword);

            if (valid_user != null)
            {
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

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });

        }
    }
}
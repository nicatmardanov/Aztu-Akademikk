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
using Microsoft.EntityFrameworkCore;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : Controller
    {
        readonly private AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        private DateTime GetDate
        {
            get
            {
                return DateTime.UtcNow.AddHours(4);
            }
        }

        private string IpAdress { get; }
        private string AInformation { get; }

        public SignInController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //POST
        [HttpPost]
        public async Task<JsonResult> Post(string email, string password)
        {
            User valid_user = await aztuAkademik.User.FirstOrDefaultAsync(x => x.Email == email && x.Password == password).ConfigureAwait(false);
            

            if (valid_user==null || !valid_user.Email.Contains("@aztu.edu.az"))
                return Json(string.Empty);



            if (valid_user != null)
            {
                //var tokenStr = GenerateJSONWebToken(valid_user);

                string userRole = valid_user.RoleId == 0 ? "User" : "Admin";

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, valid_user.Email));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, valid_user.Id.ToString()));
                claimsIdentity.AddClaim(new Claim("FullName", valid_user.FirstName + " " + valid_user.LastName));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, userRole));

                AuthenticationProperties authProperty = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddYears(1),
                    IssuedUtc = DateTime.UtcNow
                };


                
                var principial = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principial, authProperty).ConfigureAwait(false);
                
                valid_user.LastSeen = GetDate;
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);

                await Classes.TLog.Log("User", "", valid_user.Id, 4, valid_user.Id, IpAdress, AInformation).ConfigureAwait(false);

                return Json(new {
                    id=valid_user.Id, firstName=valid_user.FirstName, lastName=valid_user.LastName, patronymic=valid_user.Patronymic, 
                    email=valid_user.Email, image=valid_user.ImageAddress
                });
            }

            return Json(string.Empty);

        }

    }
}
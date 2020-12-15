﻿using System;
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
        public async Task<int> Post(User _user)
        {
            if (!_user.Email.Contains("@aztu.edu.az"))
                return 0;

            var valid_user = aztuAkademik.User.FirstOrDefault(x => x.Email == _user.Email && x.Password == _user.Password);


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

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principial, authProperty);
                await Classes.TLog.Log("User", "", _user.Id, 4, _user.Id, IpAdress, AInformation);

                return 1;
            }

            return 0;

        }

    }
}
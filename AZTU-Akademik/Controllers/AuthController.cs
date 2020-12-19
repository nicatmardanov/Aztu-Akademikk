using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AZTU_Akademik.Models;
using Microsoft.EntityFrameworkCore;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private int UserId
        {
            get
            {
                return User.Identity.IsAuthenticated ? int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value) : 0;
            }
        }

        private string UserRole
        {
            get
            {
                return User.Identity.IsAuthenticated ? User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value : string.Empty;
            }
        }

        private string UserFullName
        {
            get
            {
                return User.Identity.IsAuthenticated ? User.Claims.FirstOrDefault(x => x.Type == "FullName").Value : string.Empty;
            }
        }

        private DateTime GetDate
        {
            get
            {
                return DateTime.UtcNow.AddHours(4);
            }
        }


        [HttpGet]
        public async Task<JsonResult> Get()
        {

            if (User.Identity.IsAuthenticated)
            {
                AztuAkademikContext aztuAkademik = new AztuAkademikContext();
                var _user = await aztuAkademik.User.FirstOrDefaultAsync(x => x.Id == UserId);

                _user.LastSeen = GetDate;
                await aztuAkademik.SaveChangesAsync();
                Json(new { isAuth = true, role = UserRole, id = UserId, fullName = UserFullName });
            }

            return Json(new { isAuth = false, role = string.Empty, id = string.Empty, fullName = string.Empty });
        }
    }
}
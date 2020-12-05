using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : Controller
    {

        readonly private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private DateTime GetDate
        {
            get
            {
                return DateTime.UtcNow.AddHours(4);
            }
        }

        //POST

        [HttpPost]
        public async Task<JsonResult> Post(User _user)
        {

            if (!_user.Email.Contains("@aztu.edu.az"))
                return Json(new { res = 0 });


            _user.RoleId = 0;
            _user.CreateDate = GetDate;


            await aztuAkademik.User.AddAsync(_user);
            await aztuAkademik.SaveChangesAsync();
            return Json(new { res = 1 });
        }
    }
}
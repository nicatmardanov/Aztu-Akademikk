﻿using System;
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

        readonly AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        //POST

        [HttpPost]
        public async Task<JsonResult> Post(User _user)
        {

            if (!_user.Email.Contains("@aztu.edu.az"))
                return Json(new { res = 0 });


            _user.RoleId = 0;


            await aztuAkademik.User.AddAsync(_user);
            await aztuAkademik.SaveChangesAsync();
            return Json(new { res = 1 });
        }
    }
}
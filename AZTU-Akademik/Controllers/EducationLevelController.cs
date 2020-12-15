﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EducationLevelController : Controller
    {
        readonly private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private DateTime GetDate
        {
            get
            {
                return DateTime.UtcNow.AddHours(4);
            }
        }
        private int User_Id
        {
            get
            {
                return int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            }
        }

        private string IpAdress { get; }
        private string AInformation { get; }

        public EducationLevelController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet("EducationLevel")]
        [AllowAnonymous]
        public JsonResult EducationLevel(short id) => Json(aztuAkademik.EducationLevel.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        //POST
        [HttpPost]
        public async Task Post(EducationLevel _educationLevel)
        {
            _educationLevel.CreateDate = GetDate;
            await aztuAkademik.EducationLevel.AddAsync(_educationLevel);
            await aztuAkademik.SaveChangesAsync();

            await Classes.TLog.Log("EducationLevel", "", _educationLevel.Id, 1, User_Id, IpAdress, AInformation);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(EducationLevel _educationLevel)
        {
            if (ModelState.IsValid)
            {
                _educationLevel.UpdateDate = GetDate;
                aztuAkademik.Attach(_educationLevel);
                aztuAkademik.Entry(_educationLevel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_educationLevel).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("EducationLevel", "", _educationLevel.Id, 2, User_Id, IpAdress, AInformation);
                return 1;
            }

            return 0;
        }


        //Delete
        [HttpDelete]
        public async Task Delete(short id)
        {
            aztuAkademik.EducationLevel.FirstOrDefault(x => x.Id == id).DeleteDate = GetDate;
            aztuAkademik.EducationLevel.FirstOrDefault(x => x.Id == id).StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("EducationLevel", "", id, 3, User_Id, IpAdress, AInformation);
        }

    }
}
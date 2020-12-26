﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfessionController : Controller
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

        public ProfessionController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Profession(int id) => Json(aztuAkademik.Profession.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllProfessions")]
        [AllowAnonymous]
        public JsonResult AllProfessions() => Json(aztuAkademik.Profession.Where(x => !x.DeleteDate.HasValue).AsNoTracking());

        //POST
        [HttpPost]
        public async Task Post(Profession _profession)
        {
            _profession.CreateDate = GetDate;

            await aztuAkademik.Profession.AddAsync(_profession).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Profession", "", _profession.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(Profession _profession)
        {
            if (ModelState.IsValid)
            {
                _profession.UpdateDate = GetDate;
                aztuAkademik.Attach(_profession);
                aztuAkademik.Entry(_profession).State = EntityState.Modified;
                aztuAkademik.Entry(_profession).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Profession", "", _profession.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            Profession profession = await aztuAkademik.Profession.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            profession.DeleteDate = GetDate;
            profession.StatusId = 0;
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Profession", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }
    }
}
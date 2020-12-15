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
    public class ResearcherResearchAreaController : Controller
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

        public ResearcherResearchAreaController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet("Area")]
        [AllowAnonymous]
        public JsonResult Area(int user_id) => Json(aztuAkademik.RelResearcherResearcherArea.Where(x => x.ResearcherId == user_id).Include(x => x.Area));


        //Post
        [HttpPost]
        public async Task Post(List<RelResearcherResearcherArea> _relResearcherResearcherArea)
        {
            _relResearcherResearcherArea.ForEach(x =>
            {
                x.CreateDate = GetDate;
                x.ResearcherId = User_Id;
            });
            await aztuAkademik.RelResearcherResearcherArea.AddRangeAsync(_relResearcherResearcherArea);
            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("RelResearcherResearcherArea", "", _relResearcherResearcherArea.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(RelResearcherResearcherArea _relResearcherResearcherArea)
        {
            if (ModelState.IsValid)
            {
                _relResearcherResearcherArea.UpdateDate = GetDate;
                _relResearcherResearcherArea.ResearcherId = User_Id;
                aztuAkademik.Entry(_relResearcherResearcherArea).State = EntityState.Modified;
                aztuAkademik.Entry(_relResearcherResearcherArea).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
                await aztuAkademik.SaveChangesAsync();
                await Classes.TLog.Log("RelResearcherResearcherArea", "", _relResearcherResearcherArea.Id, 2, User_Id, IpAdress, AInformation);

                return 1;
            }
            return 0;
        }

        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.RelResearcherResearcherArea.FirstOrDefaultAsync(x => x.Id == id).Result.DeleteDate = GetDate;
            aztuAkademik.RelResearcherResearcherArea.FirstOrDefaultAsync(x => x.Id == id).Result.StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("RelResearcherResearcherArea", "", id, 3, User_Id, IpAdress, AInformation);
        }


    }
}
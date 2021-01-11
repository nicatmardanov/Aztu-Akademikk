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
    public class PatentController : Controller
    {
        public class PatentModel
        {
            public Patent Patent { get; set; }
            public IQueryable<RelPatentResearcher> RelPatentResearchers { get; set; }
            public long[] DeletedResearchers { get; set; }
        }
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

        public PatentController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Patent(int user_id) => Json(aztuAkademik.RelPatentResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Patent).ThenInclude(x => x.Researcher).
            Include(x => x.Patent).ThenInclude(x => x.Organization).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x => x.Organization).AsNoTracking());



        //POST
        [HttpPost]
        public async Task Post(PatentModel patentModel)
        {
            patentModel.Patent.CreateDate = GetDate;
            patentModel.Patent.ResearcherId = User_Id;
            await aztuAkademik.Patent.AddAsync(patentModel.Patent).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);


            await patentModel.RelPatentResearchers.ForEachAsync(x =>
            {
                x.CreateDate = GetDate;
                x.PatentId = patentModel.Patent.Id;
            }).ConfigureAwait(false);


            await aztuAkademik.RelPatentResearcher.AddRangeAsync(patentModel.RelPatentResearchers).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Patent", "", patentModel.Patent.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            await Classes.TLog.Log("RelPatentResearcher", "", patentModel.RelPatentResearchers.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(PatentModel patentModel)
        {
            if (ModelState.IsValid)
            {

                aztuAkademik.Attach(patentModel.Patent);
                aztuAkademik.Entry(patentModel.Patent).State = EntityState.Modified;
                aztuAkademik.Entry(patentModel.Patent).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(patentModel.Patent).Property(x => x.ResearcherId).IsModified = false;


                var entry = aztuAkademik.RelPatentResearcher.Where(x => patentModel.DeletedResearchers.Contains(x.Id));
                aztuAkademik.RelPatentResearcher.RemoveRange(entry);
                await Classes.TLog.Log("RelPatentResearcher", "", patentModel.DeletedResearchers, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                await patentModel.RelPatentResearchers.ForEachAsync(async x =>
                {
                    x.PatentId = patentModel.Patent.Id;

                    if (x.Id == 0)
                    {
                        x.CreateDate = GetDate;
                        await Classes.TLog.Log("RelPatentResearcher", "", x.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                    }

                    else
                    {
                        x.CreateDate = aztuAkademik.Patent.FirstOrDefault(y => y.Id == x.Id).CreateDate;
                        x.UpdateDate = GetDate;
                        await Classes.TLog.Log("RelPatentResearcher", "", x.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                    }

                }).ConfigureAwait(false);

                aztuAkademik.RelPatentResearcher.UpdateRange(patentModel.RelPatentResearchers);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Patent", "", patentModel.Patent.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }




        //DELETE
        [HttpDelete]
        public async Task Delete(int patentId)
        {
            Patent patent = await aztuAkademik.Patent.Include(x => x.RelPatentResearcher).FirstOrDefaultAsync(x => x.Id == patentId && x.ResearcherId == User_Id).
                ConfigureAwait(false);
            patent.DeleteDate = GetDate;
            patent.StatusId = 0;
            await patent.RelPatentResearcher.AsQueryable().ForEachAsync(x => x.DeleteDate = GetDate).ConfigureAwait(false);

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Patent", "", patentId, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }



    }
}
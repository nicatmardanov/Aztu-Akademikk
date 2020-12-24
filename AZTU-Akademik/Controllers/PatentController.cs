using System;
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
        [HttpGet("Patent")]
        [AllowAnonymous]
        public JsonResult Patent(int user_id) => Json(aztuAkademik.RelPatentResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Patent).ThenInclude(x => x.Researcher).
            Include(x => x.Patent).ThenInclude(x => x.Organization).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x => x.Organization));



        //POST
        [HttpPost]
        public async Task Post([FromQuery] Patent _patent, [FromQuery] IQueryable<RelPatentResearcher> _relPatentResearcher)
        {
            _patent.CreateDate = GetDate;
            _patent.ResearcherId = User_Id;
            await aztuAkademik.Patent.AddAsync(_patent).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);


            await _relPatentResearcher.ForEachAsync(x =>
            {
                x.CreateDate = GetDate;
                x.PatentId = _patent.Id;
            }).ConfigureAwait(false);


            await aztuAkademik.RelPatentResearcher.AddRangeAsync(_relPatentResearcher).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Patent", "", _patent.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            await Classes.TLog.Log("RelPatentResearcher", "", _relPatentResearcher.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery] Patent _patent, [FromQuery] IQueryable<RelPatentResearcher> _relPatentResearchers, long[] _deletedResearchers)
        {
            if (ModelState.IsValid)
            {

                aztuAkademik.Attach(_patent);
                aztuAkademik.Entry(_patent).State = EntityState.Modified;
                aztuAkademik.Entry(_patent).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_patent).Property(x => x.ResearcherId).IsModified = false;


                var entry = aztuAkademik.RelPatentResearcher.Where(x => _deletedResearchers.Contains(x.Id));
                aztuAkademik.RelPatentResearcher.RemoveRange(entry);
                await Classes.TLog.Log("RelPatentResearcher", "", _deletedResearchers, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                await _relPatentResearchers.ForEachAsync(async x =>
                {
                    x.PatentId = _patent.Id;

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

                aztuAkademik.RelPatentResearcher.UpdateRange(_relPatentResearchers);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Patent", "", _patent.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }




        //DELETE
        [HttpDelete]
        public async Task Delete(int patentId)
        {
            Patent patent = await aztuAkademik.Patent.Include(x=>x.RelPatentResearcher).FirstOrDefaultAsync(x => x.Id == patentId).ConfigureAwait(false);
            patent.DeleteDate = GetDate;
            patent.StatusId = 0;
            await patent.RelPatentResearcher.AsQueryable().ForEachAsync(x => x.DeleteDate = GetDate).ConfigureAwait(false);

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Patent", "", patentId, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }



    }
}
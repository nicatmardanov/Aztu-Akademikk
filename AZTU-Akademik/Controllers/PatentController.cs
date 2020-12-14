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

        //GET
        [HttpGet("Patent")]
        [AllowAnonymous]
        public JsonResult Patent(int user_id) => Json(aztuAkademik.RelPatentResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x=>x.Patent).ThenInclude(x=>x.Researcher).
            Include(x=>x.Patent).ThenInclude(x=>x.Organization).
            Include(x=>x.IntAuthor).Include(x=>x.ExtAuthor).ThenInclude(x=>x.Organization));



        //POST
        [HttpPost]
        public async Task Post([FromQuery] Patent _patent, [FromQuery] List<RelPatentResearcher> _relPatentResearcher)
        {
            _patent.CreateDate = GetDate;
            _patent.ResearcherId = User_Id;
            await aztuAkademik.Patent.AddAsync(_patent);
            await aztuAkademik.SaveChangesAsync();


            _relPatentResearcher.ForEach(x =>
            {
                x.CreateDate = GetDate;
                x.PatentId = _patent.Id;
            });


            await aztuAkademik.RelPatentResearcher.AddRangeAsync(_relPatentResearcher);
            await aztuAkademik.SaveChangesAsync();
        }


        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery] Patent _patent, [FromQuery] List<RelPatentResearcher> _relPatentResearchers, long[] _deletedResearchers)
        {
            if (ModelState.IsValid)
            {

                aztuAkademik.Attach(_patent);
                aztuAkademik.Entry(_patent).State = EntityState.Modified;
                aztuAkademik.Entry(_patent).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_patent).Property(x => x.ResearcherId).IsModified = false;


                var entry = aztuAkademik.RelPatentResearcher.Where(x => _deletedResearchers.Contains(x.Id));
                aztuAkademik.RelPatentResearcher.RemoveRange(entry);

                _relPatentResearchers.ForEach(x =>
                {
                    x.UpdateDate = GetDate;
                    x.PatentId = _patent.Id;

                    if (x.Id == 0)
                        x.CreateDate = GetDate;

                    else
                        x.CreateDate = aztuAkademik.Patent.FirstOrDefault(y => y.Id == x.Id).CreateDate;
                   
                });

                aztuAkademik.RelPatentResearcher.UpdateRange(_relPatentResearchers);
                await aztuAkademik.SaveChangesAsync();

                return 1;
            }
            return 0;
        }




        //DELETE
        [HttpDelete]
        public async Task Delete(int patentId)
        {
            aztuAkademik.Patent.FirstOrDefaultAsync(x => x.Id == patentId).Result.DeleteDate = GetDate;
            aztuAkademik.Patent.FirstOrDefaultAsync(x => x.Id == patentId).Result.StatusId = 0;
            aztuAkademik.Patent.FirstOrDefaultAsync(x => x.Id == patentId).Result.RelPatentResearcher.ToList().ForEach(x => x.DeleteDate = GetDate);

            await aztuAkademik.SaveChangesAsync();
        }



    }
}
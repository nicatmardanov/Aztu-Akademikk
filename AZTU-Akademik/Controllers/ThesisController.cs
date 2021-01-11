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
    public class ThesisController : Controller
    {
        public class ThesisModel
        {
            public Thesis Thesis { get; set; }
            public IQueryable<RelThesisResearcher> RelThesisResearchers { get; set; }
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

        public ThesisController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Thesis(int user_id) => Json(aztuAkademik.RelThesisResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Thesis).ThenInclude(x => x.Publisher).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x => x.Organization).AsNoTracking());


        //POST
        [HttpPost]
        public async Task Post(ThesisModel thesisModel)
        {
            thesisModel.Thesis.CreateDate = GetDate;
            thesisModel.Thesis.CreatorId = User_Id;
            await aztuAkademik.Thesis.AddAsync(thesisModel.Thesis).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);


            await thesisModel.RelThesisResearchers.ForEachAsync(x =>
            {
                x.CreateDate = GetDate;
                x.ThesisId = thesisModel.Thesis.Id;
            }).ConfigureAwait(false);


            await aztuAkademik.RelThesisResearcher.AddRangeAsync(thesisModel.RelThesisResearchers).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Thesis", "", thesisModel.Thesis.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            await Classes.TLog.Log("RelThesisResearcher", "", thesisModel.RelThesisResearchers.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }




        //PUT
        [HttpPut]
        public async Task<int> Put(ThesisModel thesisModel)
        {
            if (ModelState.IsValid)
            {

                aztuAkademik.Attach(thesisModel.Thesis);
                aztuAkademik.Entry(thesisModel.Thesis).State = EntityState.Modified;
                aztuAkademik.Entry(thesisModel.Thesis).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(thesisModel.Thesis).Property(x => x.CreatorId).IsModified = false;
                aztuAkademik.Entry(thesisModel.Thesis).Property(x => x.PublisherId).IsModified = false;


                var entry = aztuAkademik.RelThesisResearcher.Where(x => thesisModel.DeletedResearchers.Contains(x.Id));
                aztuAkademik.RelThesisResearcher.RemoveRange(entry);
                await Classes.TLog.Log("RelThesisResearcher", "", thesisModel.DeletedResearchers, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                await thesisModel.RelThesisResearchers.ForEachAsync(async x =>
                {
                    x.ThesisId = thesisModel.Thesis.Id;

                    if (x.Id == 0)
                    {
                        x.CreateDate = GetDate;
                        await Classes.TLog.Log("RelThesisResearcher", "", x.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                    }

                    else
                    {
                        x.CreateDate = aztuAkademik.Project.FirstOrDefault(y => y.Id == x.Id).CreateDate;
                        x.UpdateDate = GetDate;
                        await Classes.TLog.Log("RelThesisResearcher", "", x.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                    }

                }).ConfigureAwait(false);

                aztuAkademik.RelThesisResearcher.UpdateRange(thesisModel.RelThesisResearchers);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Thesis", "", thesisModel.Thesis.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int thesisId)
        {
            Thesis thesis = await aztuAkademik.Thesis.Include(x => x.RelThesisResearcher).
                FirstOrDefaultAsync(x => x.Id == thesisId && x.CreatorId == User_Id).
                ConfigureAwait(false);

            thesis.DeleteDate = GetDate;
            thesis.StatusId = 0;
            await thesis.RelThesisResearcher.AsQueryable().ForEachAsync(x => x.DeleteDate = GetDate).ConfigureAwait(false);

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Thesis", "", thesisId, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }
}
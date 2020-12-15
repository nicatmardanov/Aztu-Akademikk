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
        [HttpGet("Thesis")]
        [AllowAnonymous]
        public JsonResult Thesis(int user_id) => Json(aztuAkademik.RelThesisResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Thesis).ThenInclude(x => x.Publisher).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x => x.Organization));


        //POST
        [HttpPost]
        public async Task Post([FromQuery] Thesis _thesis, [FromQuery] List<RelThesisResearcher> _relThesisResearcher)
        {
            _thesis.CreateDate = GetDate;
            _thesis.CreatorId = User_Id;
            await aztuAkademik.Thesis.AddAsync(_thesis);
            await aztuAkademik.SaveChangesAsync();


            _relThesisResearcher.ForEach(x =>
            {
                x.CreateDate = GetDate;
                x.ThesisId = _thesis.Id;
            });


            await aztuAkademik.RelThesisResearcher.AddRangeAsync(_relThesisResearcher);
            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("Thesis", "", _thesis.Id, 1, User_Id, IpAdress, AInformation);
        }




        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery] Thesis _thesis, [FromQuery] List<RelThesisResearcher> _relThesisResearchers, [FromQuery] long[] _deletedResearchers)
        {
            if (ModelState.IsValid)
            {

                aztuAkademik.Attach(_thesis);
                aztuAkademik.Entry(_thesis).State = EntityState.Modified;
                aztuAkademik.Entry(_thesis).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_thesis).Property(x => x.CreatorId).IsModified = false;
                aztuAkademik.Entry(_thesis).Property(x => x.PublisherId).IsModified = false;


                var entry = aztuAkademik.RelThesisResearcher.Where(x => _deletedResearchers.Contains(x.Id));
                aztuAkademik.RelThesisResearcher.RemoveRange(entry);

                _relThesisResearchers.ForEach(x =>
                {
                    x.UpdateDate = GetDate;
                    x.ThesisId = _thesis.Id;

                    if (x.Id == 0)
                        x.CreateDate = GetDate;

                    else
                        x.CreateDate = aztuAkademik.Project.FirstOrDefault(y => y.Id == x.Id).CreateDate;

                });

                aztuAkademik.RelThesisResearcher.UpdateRange(_relThesisResearchers);
                await aztuAkademik.SaveChangesAsync();
                await Classes.TLog.Log("Thesis", "", _thesis.Id, 2, User_Id, IpAdress, AInformation);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int thesisId)
        {
            aztuAkademik.Thesis.FirstOrDefaultAsync(x => x.Id == thesisId).Result.DeleteDate = GetDate;
            aztuAkademik.Thesis.FirstOrDefaultAsync(x => x.Id == thesisId).Result.StatusId = 0;
            aztuAkademik.Thesis.FirstOrDefaultAsync(x => x.Id == thesisId).Result.RelThesisResearcher.ToList().ForEach(x => x.DeleteDate = GetDate);

            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("Thesis", "", thesisId, 3, User_Id, IpAdress, AInformation);
        }


    }
}
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
    public class ResearcherEducationController : Controller
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

        public ResearcherEducationController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult ResearcherEducation(int user_id) => Json(aztuAkademik.ResearcherEducation.Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue).
            Include(x => x.Researcher).Include(x => x.Form).Include(x => x.Level).Include(x => x.Organization).
            Include(x=>x.Country).Include(x=>x.Language).Include(x=>x.Profession).OrderByDescending(x=>x.Id).AsNoTracking());


        //POST
        [HttpPost]
        public async Task Post(ResearcherEducation _researcherEducation)
        {
            _researcherEducation.CreateDate = GetDate;
            _researcherEducation.ResearcherId = User_Id;

            await aztuAkademik.ResearcherEducation.AddAsync(_researcherEducation).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ResearcherEducation", "", _researcherEducation.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(ResearcherEducation _researcherEducation)
        {
            if (ModelState.IsValid)
            {
                _researcherEducation.UpdateDate = GetDate;
                aztuAkademik.Attach(_researcherEducation);
                aztuAkademik.Entry(_researcherEducation).State = EntityState.Modified;
                aztuAkademik.Entry(_researcherEducation).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_researcherEducation).Property(x => x.ResearcherId).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("ResearcherEducation", "", _researcherEducation.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(long id)
        {
            ResearcherEducation researcherEducation = await aztuAkademik.ResearcherEducation.FirstOrDefaultAsync(x => x.Id == id && x.ResearcherId==User_Id).
                ConfigureAwait(false);
            researcherEducation.DeleteDate = GetDate;
            researcherEducation.StatusId = 0;
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ResearcherEducation", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }
    }
}
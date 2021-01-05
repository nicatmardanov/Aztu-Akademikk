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
    public class PedagogicalExperienceController : Controller
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

        public PedagogicalExperienceController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult PedagogicalExperience(int user_id) => Json(aztuAkademik.ResearcherPosition.Where(x => x.ResearcherId == user_id).
            Include(x => x.Researcher).Include(x => x.Organization).Include(x => x.Position).Include(x => x.Department).
            OrderByDescending(x => x.Id).AsNoTracking());




        //POST
        [HttpPost]
        public async Task Post(ResearcherPosition _researcherPosition)
        {
            _researcherPosition.CreateDate = GetDate;
            _researcherPosition.ResearcherId = User_Id;

            await aztuAkademik.ResearcherPosition.AddAsync(_researcherPosition).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ResearcherPosition", "", _researcherPosition.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

       





        //PUT
        [HttpPut]
        public async Task<int> Put(ResearcherPosition _researcherPosition)
        {
            if (ModelState.IsValid)
            {
                _researcherPosition.UpdateDate = GetDate;
                aztuAkademik.Attach(_researcherPosition);
                aztuAkademik.Entry(_researcherPosition).State = EntityState.Modified;
                aztuAkademik.Entry(_researcherPosition).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_researcherPosition).Property(x => x.ResearcherId).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("ResearcherPosition", "", _researcherPosition.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }



        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            ResearcherPosition researcherPosition = await aztuAkademik.ResearcherPosition.FirstOrDefaultAsync(x => x.Id == id && x.ResearcherId==User_Id).
                ConfigureAwait(false);
            researcherPosition.DeleteDate = GetDate;
            researcherPosition.StatusId = 0;
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ResearcherPosition", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

    }
}
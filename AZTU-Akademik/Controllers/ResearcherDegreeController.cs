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
    public class ResearcherDegreeController : Controller
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

        public ResearcherDegreeController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult DegreeForUser(int user_id) => Json(aztuAkademik.RelResearcherDegree.Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue).
            Include(x => x.Degree).Include(x => x.Researcher).AsNoTracking());

        //POST
        [HttpPost]
        public async Task Post(RelResearcherDegree _relResearcherDegree)
        {
            _relResearcherDegree.CreateDate = GetDate;
            _relResearcherDegree.ResearcherId = User_Id;

            await aztuAkademik.RelResearcherDegree.AddAsync(_relResearcherDegree).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("RelResearcherDegree", "", _relResearcherDegree.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(RelResearcherDegree _relResearcherDegree)
        {
            if (ModelState.IsValid)
            {
                _relResearcherDegree.UpdateDate = GetDate;
                aztuAkademik.Attach(_relResearcherDegree);
                aztuAkademik.Entry(_relResearcherDegree).State = EntityState.Modified;
                aztuAkademik.Entry(_relResearcherDegree).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_relResearcherDegree).Property(x => x.ResearcherId).IsModified = false;


                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("RelResearcherDegree", "", _relResearcherDegree.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }

        //Delete
        [HttpDelete]
        public async Task Delete(int id)
        {
            RelResearcherDegree relResearcherDegree = await aztuAkademik.RelResearcherDegree.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            relResearcherDegree.DeleteDate = GetDate;
            relResearcherDegree.StatusId = 0;

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("RelResearcherDegree", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

    }
}
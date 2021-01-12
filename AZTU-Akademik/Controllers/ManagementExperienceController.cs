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
    public class ManagementExperienceController : Controller
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

        public ManagementExperienceController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }



        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult ManagementExperience(int user_id) => Json(aztuAkademik.ManagementExperience.Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue).
           Include(x => x.Researcher).Include(x => x.Organization).OrderByDescending(x => x.Id).AsNoTracking().
            Select(x=>new
            {
                x.Id,
                x.Name,
                x.StartDate,
                x.EndDate,
                Organization = new
                {
                    x.Organization.Id,
                    x.Organization.Name
                }
            }));


        //POST
        [HttpPost]
        public async Task Post(ManagementExperience _managementExperience)
        {
            _managementExperience.CreateDate = GetDate;
            _managementExperience.ResearcherId = User_Id;

            await aztuAkademik.ManagementExperience.AddAsync(_managementExperience).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ManagementExperience", "", _managementExperience.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(ManagementExperience _managementExperience)
        {
            if (ModelState.IsValid)
            {
                _managementExperience.UpdateDate = GetDate;
                aztuAkademik.Attach(_managementExperience);
                aztuAkademik.Entry(_managementExperience).State = EntityState.Modified;
                aztuAkademik.Entry(_managementExperience).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_managementExperience).Property(x => x.ResearcherId).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("ManagementExperience", "", _managementExperience.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            ManagementExperience managementExperience = await aztuAkademik.ManagementExperience.FirstOrDefaultAsync(x => x.Id == id && x.ResearcherId==User_Id).
                ConfigureAwait(false);

            managementExperience.DeleteDate = GetDate;
            managementExperience.StatusId = 0;
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ManagementExperience", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

    }
}
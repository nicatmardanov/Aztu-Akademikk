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
    [Authorize(Roles = "Admin")]
    public class OrganizationTypeController : Controller
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

        public OrganizationTypeController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet("OrganizationType")]
        [AllowAnonymous]
        public JsonResult OrganizationType(byte id) => Json(aztuAkademik.EducationOrganizationType.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));


        //POST
        [HttpPost]
        public async Task Post(EducationOrganizationType _educationOrganizationType)
        {
            _educationOrganizationType.CreateDate = GetDate;

            await aztuAkademik.EducationOrganizationType.AddAsync(_educationOrganizationType).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("EducationOrganizationType", "", _educationOrganizationType.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(EducationOrganizationType _educationOrganizationType)
        {
            if (ModelState.IsValid)
            {
                _educationOrganizationType.UpdateDate = GetDate;
                aztuAkademik.Attach(_educationOrganizationType);
                aztuAkademik.Entry(_educationOrganizationType).State = EntityState.Modified;
                aztuAkademik.Entry(_educationOrganizationType).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("EducationOrganizationType", "", _educationOrganizationType.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }

        //DELETE
        [HttpDelete]
        public async Task Delete(byte id)
        {
            EducationOrganizationType educationOrganizationType = await aztuAkademik.EducationOrganizationType.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            educationOrganizationType.DeleteDate = GetDate;
            educationOrganizationType.StatusId = 0;
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("EducationOrganizationType", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

    }
}
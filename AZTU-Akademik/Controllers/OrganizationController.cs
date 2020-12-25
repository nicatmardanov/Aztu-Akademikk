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
    public class OrganizationController : Controller
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

        public OrganizationController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet("Organization")]
        [AllowAnonymous]
        public JsonResult Organization(int id) => Json(aztuAkademik.EducationOrganization.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllOrganizations")]
        [AllowAnonymous]
        public JsonResult AllOrganizations() => Json(aztuAkademik.EducationOrganization.Where(x => !x.DeleteDate.HasValue).AsNoTracking());



        //POST
        [HttpPost]
        public async Task Post(EducationOrganization _educationOrganization)
        {
            _educationOrganization.CreateDate = GetDate;

            await aztuAkademik.EducationOrganization.AddAsync(_educationOrganization).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("EducationOrganization", "", _educationOrganization.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(EducationOrganization _educationOrganization)
        {
            if (ModelState.IsValid)
            {
                _educationOrganization.UpdateDate = GetDate;
                aztuAkademik.Attach(_educationOrganization);
                aztuAkademik.Entry(_educationOrganization).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_educationOrganization).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("EducationOrganization", "", _educationOrganization.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            EducationOrganization organization = await aztuAkademik.EducationOrganization.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            organization.DeleteDate = GetDate;
            organization.StatusId = 0;
            
            await Classes.TLog.Log("EducationOrganization", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
        }

    }
}
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
    public class EducationOrganizationController : Controller
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

        public EducationOrganizationController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet("EducationOrganization")]
        [AllowAnonymous]
        public JsonResult EducationOrganization(int id) => Json(aztuAkademik.EducationOrganization.Include(x => x.Type).FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllEducationOrganizations")]
        [AllowAnonymous]
        public JsonResult AllEducationOrganizations() => Json(aztuAkademik.EducationOrganization.Where(x => !x.DeleteDate.HasValue).Include(x => x.Type));

        //POST
        [HttpPost]
        public async Task Post(EducationOrganization _educationOrganization)
        {
            _educationOrganization.CreateDate = GetDate;

            await aztuAkademik.EducationOrganization.AddAsync(_educationOrganization);
            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("EducationOrganization", "", _educationOrganization.Id, 1, User_Id, IpAdress, AInformation);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(EducationOrganization _educationOrganization)
        {
            if (ModelState.IsValid)
            {
                _educationOrganization.UpdateDate = GetDate;

                aztuAkademik.Attach(_educationOrganization);
                aztuAkademik.Entry(_educationOrganization).State = EntityState.Modified;
                aztuAkademik.Entry(_educationOrganization).Property(x => x.CreateDate).IsModified = false;
                await aztuAkademik.SaveChangesAsync();
                await Classes.TLog.Log("EducationOrganization", "", _educationOrganization.Id, 2, User_Id, IpAdress, AInformation);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.EducationOrganization.FirstOrDefaultAsync(x => x.Id == id).Result.DeleteDate = GetDate;
            aztuAkademik.EducationOrganization.FirstOrDefaultAsync(x => x.Id == id).Result.StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("EducationOrganization", "", id, 3, User_Id, IpAdress, AInformation);
        }


    }
}
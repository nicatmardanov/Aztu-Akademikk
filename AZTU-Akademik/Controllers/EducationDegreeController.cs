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
    public class EducationDegreeController : Controller
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

        public EducationDegreeController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet("EducationDegree")]
        [AllowAnonymous]
        public JsonResult EducationDegree(int id) => Json(aztuAkademik.EducationDegree.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllEducationDegrees")]
        [AllowAnonymous]
        public JsonResult AllEducationDegrees() => Json(aztuAkademik.EducationDegree.FirstOrDefault(x => !x.DeleteDate.HasValue));


        //POST
        [HttpPost]
        public async Task Post(EducationDegree _educationDegree)
        {
            _educationDegree.CreateDate = GetDate;

            await aztuAkademik.EducationDegree.AddAsync(_educationDegree).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("EducationDegree", "", _educationDegree.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(EducationDegree _educationDegree)
        {
            if (ModelState.IsValid)
            {
                _educationDegree.UpdateDate = GetDate;
                aztuAkademik.Attach(_educationDegree);
                aztuAkademik.Entry(_educationDegree).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_educationDegree).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("EducationDegree", "", _educationDegree.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            EducationDegree educationDegree = await aztuAkademik.EducationDegree.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            educationDegree.DeleteDate = GetDate;
            educationDegree.StatusId = 0;

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("EducationDegree", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }
}
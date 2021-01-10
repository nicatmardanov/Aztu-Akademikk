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
    public class EducationFormController : Controller
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

        public EducationFormController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult EducationForm(short id) => Json(aztuAkademik.EducationForm.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        //GET
        [HttpGet("AllEducationForms")]
        [AllowAnonymous]
        public JsonResult AllEducationForms() => Json(aztuAkademik.EducationForm.AsNoTracking().
            Where(x => !x.DeleteDate.HasValue).OrderByDescending(x => x.Id).
            Select(x => new
            {
                x.Id,
                x.Name,
                x.Description
            }));


        //POST
        [HttpPost]
        public async Task Post(EducationForm _educationForm)
        {
            _educationForm.CreateDate = GetDate;
            await aztuAkademik.EducationForm.AddAsync(_educationForm).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("EducationForm", "", _educationForm.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(EducationForm _educationForm)
        {
            if (ModelState.IsValid)
            {
                _educationForm.UpdateDate = GetDate;
                aztuAkademik.Attach(_educationForm);
                aztuAkademik.Entry(_educationForm).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_educationForm).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("EducationForm", "", _educationForm.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                return 1;
            }

            return 0;
        }


        //Delete
        [HttpDelete]
        public async Task Delete(short id)
        {
            EducationForm educationForm = await aztuAkademik.EducationForm.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            educationForm.DeleteDate = GetDate;
            educationForm.StatusId = 0;

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("EducationForm", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }
}
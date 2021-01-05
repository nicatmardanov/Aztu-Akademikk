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
    public class FacultyController : Controller
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

        public FacultyController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Faculty(int id) => Json(aztuAkademik.Faculty.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllFaculties")]
        [AllowAnonymous]
        public JsonResult AllFaculties() => Json(aztuAkademik.Faculty.Where(x => !x.DeleteDate.HasValue).OrderByDescending(x => x.Id).AsNoTracking());



        //POST
        [HttpPost]
        public async Task Post(Faculty _faculty)
        {
            _faculty.CreateDate = GetDate;

            await aztuAkademik.Faculty.AddAsync(_faculty).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Faculty", "", _faculty.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(Faculty _faculty)
        {
            if (ModelState.IsValid)
            {
                _faculty.UpdateDate = GetDate;
                aztuAkademik.Attach(_faculty);
                aztuAkademik.Entry(_faculty).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_faculty).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Faculty", "", _faculty.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            Faculty faculty = await aztuAkademik.Faculty.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            faculty.DeleteDate = GetDate;
            faculty.StatusId = 0;
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Faculty", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

    }
}
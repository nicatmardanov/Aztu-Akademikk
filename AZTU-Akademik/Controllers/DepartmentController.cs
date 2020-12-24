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
    public class DepartmentController : Controller
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

        public DepartmentController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet("Department")]
        [AllowAnonymous]
        public JsonResult Department(int id) => Json(aztuAkademik.Department.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllDepartments")]
        [AllowAnonymous]
        public JsonResult AllDepartments() => Json(aztuAkademik.Department.Where(x => !x.DeleteDate.HasValue));



        //POST
        [HttpPost]
        public async Task Post(Department _department)
        {
            _department.CreateDate = GetDate;

            await aztuAkademik.Department.AddAsync(_department).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Department", "", _department.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

        }

        //PUT
        [HttpPut]
        public async Task<int> Put(Department _department)
        {
            if (ModelState.IsValid)
            {
                _department.UpdateDate = GetDate;
                aztuAkademik.Attach(_department);
                aztuAkademik.Entry(_department).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_department).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Department", "", _department.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            Department department = await aztuAkademik.Department.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            department.DeleteDate = GetDate;
            department.StatusId = 0;
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Department", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }
    }
}
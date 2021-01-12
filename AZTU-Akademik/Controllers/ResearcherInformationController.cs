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
    public class ResearcherInformationController : Controller
    {
        public class ExperienceModel
        {
            public static int Id { get; set; }
            public static string Name { get; set; }
        }

        public class DepartmentModel
        {
            public static int Id { get; set; }
            public static string Name { get; set; }
        }

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

        public ResearcherInformationController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet("EditInformation")]
        [AllowAnonymous]
        public int EditInformation() => User.Identity.IsAuthenticated ? User_Id : 0;

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> Information(int user_id)
        {
            User _user = aztuAkademik.User.
            AsNoTracking().FirstOrDefault(x => x.Id == user_id && !x.DeleteDate.HasValue);

            var researcherPosition = await aztuAkademik.ResearcherPosition.AsNoTracking().Where(x => !x.EndDate.HasValue && !x.DeleteDate.HasValue).
                Include(x => x.Organization).Include(x => x.Department).
                Select(x => new
                {
                    x.Organization.Id,
                    x.Organization.Name,
                    x.StartDate,
                    DepartmentId = x.Department.Id,
                    DepartmentName = x.Department.Name,
                }).
                OrderByDescending(x => x.Id).
                FirstOrDefaultAsync().
                ConfigureAwait(false);

            var managementExperience = await aztuAkademik.ManagementExperience.AsNoTracking().Where(x => !x.EndDate.HasValue && !x.DeleteDate.HasValue).
                Include(x => x.Organization).
                Select(x => new
                {
                    x.Organization.Id,
                    x.Organization.Name,
                    x.StartDate
                }).
                OrderByDescending(x => x.Id).
                FirstOrDefaultAsync().
                ConfigureAwait(false);


            if (managementExperience == null && researcherPosition != null)
            {
                ExperienceModel.Id = researcherPosition.Id;
                ExperienceModel.Name = researcherPosition.Name;
            }
            else if (managementExperience != null && researcherPosition == null)
            {
                ExperienceModel.Id = managementExperience.Id;
                ExperienceModel.Name = managementExperience.Name;
            }
            else if (managementExperience != null && researcherPosition != null)
            {
                bool e_check = managementExperience.StartDate.Value.CompareTo(researcherPosition.StartDate.Value) == 1 ? true : false;

                if (managementExperience.StartDate.Value.CompareTo(researcherPosition.StartDate.Value) == 1)
                {
                    ExperienceModel.Id = managementExperience.Id;
                    ExperienceModel.Name = managementExperience.Name;
                }
                else
                {
                    ExperienceModel.Id = researcherPosition.Id;
                    ExperienceModel.Name = researcherPosition.Name;
                }
            }

            DepartmentModel.Id = researcherPosition!=null ? researcherPosition.DepartmentId : 0;
            DepartmentModel.Name = researcherPosition!=null ? researcherPosition.DepartmentName : null;



            return Json(new
            {
                _user.Id,
                _user.FirstName,
                _user.LastName,
                _user.Patronymic,
                _user.ImageAddress,
                _user.Email,
                Organization = new
                {
                    ExperienceModel.Id,
                    ExperienceModel.Name
                },
                Department = new
                {
                    DepartmentModel.Id,
                    DepartmentModel.Name
                }
            });

        }



        //Put
        [HttpPut("InformationPut")]
        public async Task InformationPut(User _user)
        {
            //if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
            //    _user.ImageAddress = await Classes.FileSave.Save(Request.Form.Files[0], 4);
            if (ModelState.IsValid)
            {
                _user.UpdateDate = GetDate;
                aztuAkademik.Attach(_user);
                aztuAkademik.Entry(_user).State = EntityState.Modified;
                aztuAkademik.Entry(_user).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_user).Property(x => x.RoleId).IsModified = false;
                aztuAkademik.Entry(_user).Property(x => x.StatusId).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("User", "", _user.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            }
        }

        [HttpPut("ImagePut")]
        public async Task ImagePut()
        {
            if (ModelState.IsValid)
            {
                User _user = await aztuAkademik.User.FirstOrDefaultAsync(x => x.Id == User_Id && !x.DeleteDate.HasValue).ConfigureAwait(false);
                _user.UpdateDate = GetDate;

                if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
                    _user.ImageAddress = await Classes.FileSave.Save(Request.Form.Files[0], 4).ConfigureAwait(false);

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("User", "Only Image", _user.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            }
        }

        [HttpPut("ImageDelete")]
        public async Task ImageDelete()
        {
            if (ModelState.IsValid)
            {
                User _user = await aztuAkademik.User.FirstOrDefaultAsync(x => x.Id == User_Id && !x.DeleteDate.HasValue).ConfigureAwait(false);
                _user.UpdateDate = GetDate;
                _user.ImageAddress = string.Empty;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("User", "Only Image", _user.Id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            }
        }


        //DELETE
        [HttpDelete("UserDelete")]
        public async Task UserDelete(int id)
        {
            User user = await aztuAkademik.User.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            user.DeleteDate = GetDate;
            user.StatusId = 0;

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("User", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }
    }
}
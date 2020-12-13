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


        //GET
        [HttpGet]
        [AllowAnonymous]
        public int EditInformationPage() => User_Id;

        [HttpGet("Information")]
        [AllowAnonymous]
        public JsonResult Information(int user_id) => Json(aztuAkademik.User.
            Include(x => x.Nationality).Include(x => x.Citizenship).
            FirstOrDefault(x => x.Id == user_id && !x.DeleteDate.HasValue));



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

                await aztuAkademik.SaveChangesAsync();
            }
        }

        [HttpPut("ImagePut")]
        public async Task ImagePut()
        {
            if (ModelState.IsValid)
            {
                User _user = await aztuAkademik.User.FirstOrDefaultAsync(x => x.Id == User_Id && !x.DeleteDate.HasValue);
                _user.UpdateDate = GetDate;

                if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
                    _user.ImageAddress = await Classes.FileSave.Save(Request.Form.Files[0], 4);

                await aztuAkademik.SaveChangesAsync();
            }
        }

        [HttpPut("ImageDelete")]
        public async Task ImageDelete()
        {
            if (ModelState.IsValid)
            {
                User _user = await aztuAkademik.User.FirstOrDefaultAsync(x => x.Id == User_Id && !x.DeleteDate.HasValue);
                _user.UpdateDate = GetDate;
                _user.ImageAddress = string.Empty;

                await aztuAkademik.SaveChangesAsync();
            }
        }


        //DELETE
        [HttpDelete("UserDelete")]
        public async Task UserDelete(int id)
        {
            aztuAkademik.User.FirstOrDefaultAsync(x => x.Id == id).Result.DeleteDate = GetDate;
            aztuAkademik.User.FirstOrDefaultAsync(x => x.Id == id).Result.StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
        }
    }
}
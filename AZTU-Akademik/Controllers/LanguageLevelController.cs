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
    public class LanguageLevelController : Controller
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

        public LanguageLevelController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Level(short id) => Json(aztuAkademik.LanguageLevels.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllLevels")]
        [AllowAnonymous]
        public JsonResult AllLevels() => Json(aztuAkademik.LanguageLevels.Where(x => !x.DeleteDate.HasValue).OrderByDescending(x => x.Id).AsNoTracking());


        //POST
        [HttpPost]
        public async Task Post(LanguageLevels _languageLevels)
        {
            _languageLevels.CreateDate = GetDate;

            await aztuAkademik.LanguageLevels.AddAsync(_languageLevels).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);

            await Classes.TLog.Log("LanguageLevels", "", _languageLevels.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(LanguageLevels _languageLevels)
        {
            if (ModelState.IsValid)
            {
                _languageLevels.UpdateDate = GetDate;
                aztuAkademik.Attach(_languageLevels);
                aztuAkademik.Entry(_languageLevels).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_languageLevels).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);

                await Classes.TLog.Log("LanguageLevels", "", _languageLevels.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }


        //Delete
        [HttpDelete]
        public async Task Delete(short id)
        {
            LanguageLevels languageLevels = await aztuAkademik.LanguageLevels.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            languageLevels.DeleteDate = GetDate;
            languageLevels.StatusId = 0;
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("LanguageLevels", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

    }
}
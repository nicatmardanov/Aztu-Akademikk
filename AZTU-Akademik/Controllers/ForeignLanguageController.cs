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
    public class ForeignLanguageController : Controller
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

        public ForeignLanguageController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet("Language")]
        [AllowAnonymous]
        public JsonResult Language(int id) => Json(aztuAkademik.Language.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllLanguages")]
        [AllowAnonymous]
        public JsonResult AllLanguages() => Json(aztuAkademik.Language.Where(x => !x.DeleteDate.HasValue));



        //POST
        [HttpPost]
        public async Task Post(Language _language)
        {
            _language.CreateDate = GetDate;
            await aztuAkademik.Language.AddAsync(_language);
            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("Language", "", _language.Id, 1, User_Id, IpAdress, AInformation);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(Language _language)
        {
            if (ModelState.IsValid)
            {
                _language.UpdateDate = GetDate;
                aztuAkademik.Attach(_language);
                aztuAkademik.Entry(_language).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_language).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
                await Classes.TLog.Log("Language", "", _language.Id, 2, User_Id, IpAdress, AInformation);

                return 1;
            }

            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(short id)
        {
            Language language = await aztuAkademik.Language.FirstOrDefaultAsync(x => x.Id == id);
            language.DeleteDate = GetDate;
            language.StatusId = 0;
            
            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("Language", "", id, 3, User_Id, IpAdress, AInformation);
        }

    }
}
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
    public class ResearcherLanguageController : Controller
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

        public ResearcherLanguageController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet("ResearcherLanguages")]
        [AllowAnonymous]
        public JsonResult ResearcherLanguages(int user_id) => Json(aztuAkademik.ResearcherLanguage.
            Include(x => x.Researcher).Include(x => x.Language).Include(x => x.Level).Include(x => x.File).
            Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue));



        //POST
        [HttpPost]
        public async Task Post(ResearcherLanguage _researcherLanguage)
        {
            if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
            {
                File _file = new File
                {
                    Name = await Classes.FileSave.Save(Request.Form.Files[0], 1),
                    Type = 2,
                    CreateDate = GetDate,
                    StatusId = 1,
                    UserId = User_Id
                };

                await aztuAkademik.File.AddAsync(_file);
                await aztuAkademik.SaveChangesAsync();

                _researcherLanguage.CreateDate = GetDate;
                _researcherLanguage.FileId = _file.Id;
                _researcherLanguage.ResearcherId = User_Id;


                await aztuAkademik.SaveChangesAsync();
                await Classes.TLog.Log("ResearcherLanguage", "", _researcherLanguage.Id, 1, User_Id, IpAdress, AInformation);
                await Classes.TLog.Log("File", "", _file.Id, 1, User_Id, IpAdress, AInformation);

            }
        }

        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery]ResearcherLanguage _researcherLanguage, [FromQuery] bool fileChange)
        {
            if (ModelState.IsValid)
            {
                if (fileChange)
                {
                    File _file = await aztuAkademik.File.FirstOrDefaultAsync(x => x.Id == _researcherLanguage.FileId);
                    if (!string.IsNullOrEmpty(_file.Name))
                        System.IO.File.Delete(_file.Name[1..]);

                    _file.Name = await Classes.FileSave.Save(Request.Form.Files[0], 1);
                    _file.UpdateDate = GetDate;
                    await Classes.TLog.Log("File", "", _file.Id, 2, User_Id, IpAdress, AInformation);
                }

                _researcherLanguage.UpdateDate = GetDate;
                aztuAkademik.Entry(_researcherLanguage).State = EntityState.Modified;
                aztuAkademik.Entry(_researcherLanguage).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_researcherLanguage).Property(x => x.ResearcherId).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
                await Classes.TLog.Log("ResearcherLanguage", "", _researcherLanguage.Id, 2, User_Id, IpAdress, AInformation);
                return 1;
            }

            return 0;
        }

        //Delete
        [HttpDelete]
        public async Task Delete(int id)
        {
            ResearcherLanguage researcherLanguage = await aztuAkademik.ResearcherLanguage.FirstOrDefaultAsync(x => x.Id == id);
            researcherLanguage.DeleteDate = GetDate;
            researcherLanguage.StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("ResearcherLanguage", "", id, 3, User_Id, IpAdress, AInformation);
        }


    }
}
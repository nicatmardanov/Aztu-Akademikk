﻿using System;
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
        public class ResearcherLanguageModel{
            public ResearcherLanguage ResearcherLanguage { get; set; }
            public bool FileChange { get; set; }
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

        public ResearcherLanguageController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult ResearcherLanguages(int user_id) => Json(aztuAkademik.ResearcherLanguage.
            Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue).
            Include(x => x.Researcher).Include(x => x.Language).Include(x => x.Level).Include(x => x.File).
            OrderByDescending(x => x.Id).AsNoTracking().
            Select(x => new
            {
                x.Id,
                Language = new
                {
                    x.Language.Id,
                    x.Language.Name
                },
                Level = new
                {
                    x.Level.Id,
                    x.Level.Name
                }
            }));



        //POST
        [HttpPost]
        public async Task Post([FromForm]ResearcherLanguage _researcherLanguage)
        {
            if (Request.Form.Files.Count > 0)
            {
                File _file = new File
                {
                    Name = await Classes.FileSave.Save(Request.Form.Files[0], 1).ConfigureAwait(false),
                    Type = 2,
                    CreateDate = GetDate,
                    StatusId = 1,
                    UserId = User_Id
                };

                await aztuAkademik.File.AddAsync(_file).ConfigureAwait(false);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                _researcherLanguage.FileId = _file.Id;

                await Classes.TLog.Log("File", "", _file.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            }

            _researcherLanguage.CreateDate = GetDate;
            _researcherLanguage.ResearcherId = User_Id;

            await aztuAkademik.ResearcherLanguage.AddAsync(_researcherLanguage).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ResearcherLanguage", "", _researcherLanguage.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put([FromForm]ResearcherLanguageModel researcherLanguageModel)
        {
            if (ModelState.IsValid)
            {
                if (researcherLanguageModel.FileChange)
                {
                    File _file = await aztuAkademik.File.FirstOrDefaultAsync(x => x.Id == researcherLanguageModel.ResearcherLanguage.FileId).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(_file.Name))
                        System.IO.File.Delete(_file.Name[1..]);

                    _file.Name = await Classes.FileSave.Save(Request.Form.Files[0], 1).ConfigureAwait(false);
                    _file.UpdateDate = GetDate;
                    await Classes.TLog.Log("File", "", _file.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                }

                researcherLanguageModel.ResearcherLanguage.UpdateDate = GetDate;
                aztuAkademik.Entry(researcherLanguageModel.ResearcherLanguage).State = EntityState.Modified;
                aztuAkademik.Entry(researcherLanguageModel.ResearcherLanguage).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(researcherLanguageModel.ResearcherLanguage).Property(x => x.ResearcherId).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("ResearcherLanguage", "", researcherLanguageModel.ResearcherLanguage.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                return 1;
            }

            return 0;
        }

        //Delete
        [HttpDelete]
        public async Task Delete(int id)
        {
            ResearcherLanguage researcherLanguage = await aztuAkademik.ResearcherLanguage.FirstOrDefaultAsync(x => x.Id == id && x.ResearcherId == User_Id).
                ConfigureAwait(false);
            researcherLanguage.DeleteDate = GetDate;
            researcherLanguage.StatusId = 0;

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ResearcherLanguage", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }
}
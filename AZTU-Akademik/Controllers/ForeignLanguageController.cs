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

        //GET
        [HttpGet("Language")]
        [AllowAnonymous]
        public JsonResult Language(int id) => Json(aztuAkademik.Language.FirstOrDefault(x=> x.Id==id && !x.DeleteDate.HasValue));

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
                
                return 1;
            }

            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(short id)
        {
            aztuAkademik.Language.FirstOrDefaultAsync(x => x.Id == id).Result.DeleteDate = GetDate;
            aztuAkademik.Language.FirstOrDefaultAsync(x => x.Id == id).Result.StatusId = 0;
            await aztuAkademik.SaveChangesAsync();
        }

    }
}
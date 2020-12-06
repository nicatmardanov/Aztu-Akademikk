﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResearcherEducationController : Controller
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
        [HttpGet("ResearcherEducation")]
        public JsonResult ResearcherEducation(int user_id) => Json(aztuAkademik.ResearcherEducation.Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue).
            Include(x => x.Researcher).Include(x => x.Form).Include(x => x.Level).Include(x => x.Organization).
            Include(x=>x.Country).Include(x=>x.Language).Include(x=>x.Profession).OrderByDescending(x=>x.Id));


        //POST
        [HttpPost]
        public async Task Post(ResearcherEducation _researcherEducation)
        {
            _researcherEducation.CreateDate = GetDate;
            _researcherEducation.ResearcherId = User_Id;

            await aztuAkademik.ResearcherEducation.AddAsync(_researcherEducation);
            await aztuAkademik.SaveChangesAsync();
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(ResearcherEducation _researcherEducation)
        {
            if (ModelState.IsValid)
            {
                _researcherEducation.UpdateDate = GetDate;
                aztuAkademik.Attach(_researcherEducation);
                aztuAkademik.Entry(_researcherEducation).State = EntityState.Modified;
                aztuAkademik.Entry(_researcherEducation).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_researcherEducation).Property(x => x.ResearcherId).IsModified = false;

                await aztuAkademik.SaveChangesAsync();

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(long id)
        {
            aztuAkademik.ResearcherEducation.FirstOrDefault(x => x.Id == id).DeleteDate = GetDate;
            aztuAkademik.ResearcherEducation.FirstOrDefault(x => x.Id == id).StatusId = 0;
            await aztuAkademik.SaveChangesAsync();
        }
    }
}
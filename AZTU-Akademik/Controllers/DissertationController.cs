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
    public class DissertationController : Controller
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
        [HttpGet("Dissertation")]
        public JsonResult Dissertation(int user_id) => Json(aztuAkademik.ResearcherEducation.Include(x=>x.Dissertation).Where(x=>x.ResearcherId==user_id).Select(x=>x.Dissertation));

        
        [HttpGet("AddDissertation")]
        public JsonResult AddDissertation(int research_education_id) => Json(research_education_id);


        //POST
        [HttpPost]
        public async Task Post(Dissertation _dissertation)
        {

            if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
            {
                File _file = new File
                {
                    Name = await Classes.FileSave.Save(Request.Form.Files[0], 0),
                    Type = 1,
                    CreateDate = GetDate,
                    StatusId = 1,
                    UserId = User_Id
                };


                await aztuAkademik.File.AddAsync(_file);
                await aztuAkademik.SaveChangesAsync();



                _dissertation.FileId = _file.Id;
                _dissertation.CreateDate = GetDate;

                await aztuAkademik.Dissertation.AddAsync(_dissertation);
                await aztuAkademik.SaveChangesAsync();
            }

        }


        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery] Dissertation _dissertation, [FromQuery] bool fileChange)
        {
            if (ModelState.IsValid)
            {
                if (fileChange)
                {
                    File _file = await aztuAkademik.File.FirstOrDefaultAsync(x => x.Id == _dissertation.FileId);
                    if (!string.IsNullOrEmpty(_file.Name))
                        System.IO.File.Delete(_file.Name[1..]);

                    _file.Name = await Classes.FileSave.Save(Request.Form.Files[0], 0);
                    _file.UpdateDate = GetDate;

                }
                _dissertation.UpdateDate = GetDate;
                aztuAkademik.Entry(_dissertation).State = EntityState.Modified;
                aztuAkademik.Entry(_dissertation).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.Dissertation.FirstOrDefaultAsync(x => x.Id == id).Result.StatusId = 0;
            aztuAkademik.Dissertation.FirstOrDefaultAsync(x => x.Id == id).Result.DeleteDate = GetDate;

            await aztuAkademik.SaveChangesAsync();
        }


    }
}
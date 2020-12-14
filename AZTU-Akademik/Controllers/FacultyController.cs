﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FacultyController : Controller
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
        [HttpGet("Faculty")]
        [AllowAnonymous]
        public JsonResult Faculty(int id) => Json(aztuAkademik.Faculty.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllFaculties")]
        [AllowAnonymous]
        public JsonResult AllFaculties() => Json(aztuAkademik.Faculty.Where(x => !x.DeleteDate.HasValue));



        //POST
        [HttpPost]
        public async Task Post(Faculty _faculty)
        {
            _faculty.CreateDate = GetDate;

            await aztuAkademik.Faculty.AddAsync(_faculty);
            await aztuAkademik.SaveChangesAsync();
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(Faculty _faculty)
        {
            if (ModelState.IsValid)
            {
                _faculty.UpdateDate = GetDate;
                aztuAkademik.Attach(_faculty);
                aztuAkademik.Entry(_faculty).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_faculty).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.Faculty.FirstOrDefault(x => x.Id == id).DeleteDate = GetDate;
            aztuAkademik.Faculty.FirstOrDefault(x => x.Id == id).StatusId = 0;
            await aztuAkademik.SaveChangesAsync();
        }

    }
}
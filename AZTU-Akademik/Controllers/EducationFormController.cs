using System;
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
    [Authorize(Roles ="Admin")]
    public class EducationFormController : Controller
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
        [HttpGet("EducationForm")]
        [AllowAnonymous]
        public JsonResult EducationForm(short id) => Json(aztuAkademik.EducationForm.FirstOrDefault(x=>x.Id==id && !x.DeleteDate.HasValue));


        //POST
        [HttpPost]
        public async Task Post(EducationForm _educationForm)
        {
            _educationForm.CreateDate = GetDate;
            await aztuAkademik.EducationForm.AddAsync(_educationForm);
            await aztuAkademik.SaveChangesAsync();
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(EducationForm _educationForm)
        {
            if (ModelState.IsValid)
            {
                _educationForm.UpdateDate = GetDate;
                aztuAkademik.Attach(_educationForm);
                aztuAkademik.Entry(_educationForm).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_educationForm).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
                return 1;
            }

            return 0;
        }


        //Delete
        [HttpDelete]
        public async Task Delete(short id)
        {
            aztuAkademik.EducationForm.FirstOrDefault(x => x.Id == id).DeleteDate = GetDate;
            aztuAkademik.EducationForm.FirstOrDefault(x => x.Id == id).StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
        }


    }
}
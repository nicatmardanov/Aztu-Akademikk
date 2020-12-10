using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationDegreeController : Controller
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
        [HttpGet("EducationDegree")]
        public JsonResult EducationDegree(int id) => Json(aztuAkademik.EducationDegree.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllEducationDegrees")]
        public JsonResult AllEducationDegrees() => Json(aztuAkademik.EducationDegree.FirstOrDefault(x => !x.DeleteDate.HasValue));


        //POST
        [HttpPost]
        public async Task Post(EducationDegree _educationDegree)
        {
            _educationDegree.CreateDate = GetDate;

            await aztuAkademik.EducationDegree.AddAsync(_educationDegree);
            await aztuAkademik.SaveChangesAsync();
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(EducationDegree _educationDegree)
        {
            if (ModelState.IsValid)
            {
                _educationDegree.UpdateDate = GetDate;
                aztuAkademik.Attach(_educationDegree);
                aztuAkademik.Entry(_educationDegree).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_educationDegree).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.EducationDegree.FirstOrDefault(x => x.Id == id).DeleteDate = GetDate;
            aztuAkademik.EducationDegree.FirstOrDefault(x => x.Id == id).StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
        }


    }
}
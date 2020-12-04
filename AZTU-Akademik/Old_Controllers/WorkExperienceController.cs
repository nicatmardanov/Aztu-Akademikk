using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AZTU_Akademik.Models;
using System.Security.Claims;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkExperienceController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);


        [HttpGet]
        public JsonResult GetAll(int id)
        {
            var education_level = aztuAkademik.TehsilSeviyye.Where(x => x.ArasdirmaciId == id); 

            return Json(new { academic = new { dos = education_level.Select(x => x.ElmlerDoktoruNavigation), eos = education_level.Select(x => x.ElmlerNamizedi), master = education_level.Select(x => x.Magistr) }, managerial_experience = aztuAkademik.IsTecrubesi.Where(x => x.ArasdirmaciId == id).OrderByDescending(x => x.Id) });
        }

        //[HttpGet("WorkExperience")]
        //public JsonResult WorkExperience(int id)
        //{
        //    return Json(aztuAkademik.IsTecrubesi.FirstOrDefault(x => x.Id == id));
        //}


        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Post(IsTecrubesi work_experience)
        {
            work_experience.ArasdirmaciId = User_Id;
            await aztuAkademik.IsTecrubesi.AddAsync(work_experience);
            await aztuAkademik.SaveChangesAsync();

            return Json(new { res = 1 });
        }


        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Put(IsTecrubesi work_experience)
        {
            if (ModelState.IsValid)
            {
                work_experience.ArasdirmaciId = User_Id;
                aztuAkademik.IsTecrubesi.Update(work_experience);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }

        [HttpDelete]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var work_experience = aztuAkademik.IsTecrubesi.FirstOrDefault(x => x.Id == id && x.ArasdirmaciId == User_Id);

            if (work_experience != null)
            {
                aztuAkademik.IsTecrubesi.Remove(work_experience);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });

        }
    }
}
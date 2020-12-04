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
    public class AchievementsController : Controller
    {

        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);


        [HttpGet]
        public JsonResult GetAll(int id) => Json(new { awards = aztuAkademik.Mukafatlar.Where(x => x.ArasdirmaciId == id), patents = aztuAkademik.Patentler.Where(x => x.ArasdirmaciId == id) });


        [HttpGet("award")]
        public JsonResult GetAward(int id)
        {

            var achievement = aztuAkademik.Mukafatlar.FirstOrDefault(x => x.Id == id);
            return Json(achievement);

        }

        [HttpGet("patent")]
        public JsonResult GetPatent(int id)
        {
            var patent = aztuAkademik.Patentler.FirstOrDefault(x => x.Id == id);

            return Json(patent);
        }

        [HttpPost("{award}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> PostAwards(Mukafatlar award)
        {
            award.ArasdirmaciId = User_Id;
            await aztuAkademik.Mukafatlar.AddAsync(award);
            await aztuAkademik.SaveChangesAsync();
            return Json(new { res = 1 });
        }

        [HttpPost("{patent}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> PostPatents(Patentler patent)
        {
            patent.ArasdirmaciId = User_Id;
            await aztuAkademik.Patentler.AddAsync(patent);
            await aztuAkademik.SaveChangesAsync();

            return Json(new { res = 1 });
        }

        [HttpPut("{award}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> PutAwards(Mukafatlar award)
        {
            if (ModelState.IsValid)
            {
                award.ArasdirmaciId = User_Id;
                aztuAkademik.Mukafatlar.Update(award);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }
            return Json(new { res = 0 });
        }


        [HttpPut("{patent}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> PutPatents(Patentler patent)
        {
            if (ModelState.IsValid)
            {
                patent.ArasdirmaciId = User_Id;
                aztuAkademik.Patentler.Update(patent);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }


        [HttpDelete("{award}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> DeleteAwards(int id)
        {
            var award = aztuAkademik.Mukafatlar.FirstOrDefault(x => x.Id == id && x.ArasdirmaciId == User_Id);

            if (award != null)
            {
                aztuAkademik.Mukafatlar.Remove(award);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }


        [HttpDelete("{patent}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> DeletePatents(int id)
        {
            var patent = aztuAkademik.Patentler.FirstOrDefault(x => x.Id == id && x.ArasdirmaciId == User_Id);

            if (patent != null)
            {
                aztuAkademik.Patentler.Remove(patent);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }

    }
}
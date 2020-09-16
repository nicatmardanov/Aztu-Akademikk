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
    public class AnnouncementsController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);


        [HttpGet("GetAll")]
        public JsonResult GetAll() => Json(aztuAkademik.Elanlar);

        [HttpGet("Get")]
        public JsonResult Get() => Json(aztuAkademik.Elanlar.Where(x => x.ArasdirmaciId == User_Id));


        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        public async Task<JsonResult> Post(Elanlar announcement)
        {
            announcement.ArasdirmaciId = User_Id;

            aztuAkademik.Elanlar.Add(announcement);
            await aztuAkademik.SaveChangesAsync();

            return Json(new { res = 1 });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPut]
        public async Task<JsonResult> Put(Elanlar announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.ArasdirmaciId = User_Id;
                aztuAkademik.Elanlar.Update(announcement);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }


        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpDelete]
        public async Task<JsonResult> Delete(int id)
        {
            var announcement = aztuAkademik.Elanlar.FirstOrDefault(x => x.Id == id);
            if (announcement != null)
            {
                aztuAkademik.Elanlar.Remove(announcement);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }


    }
}
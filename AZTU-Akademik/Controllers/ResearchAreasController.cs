using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AZTU_Akademik.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResearchAreasController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet("id")]
        public IEnumerable<ArasdirmaSaheleri> GetAll(int id) => aztuAkademik.ArasdirmaSaheleri.Where(x => x.ArasdirmaciId == id).Include(x=>x.Kafedra).OrderByDescending(x => x.Id).AsNoTracking();

        [HttpGet("r_id")]
        public JsonResult Get(int r_id)
        {
            return Json(aztuAkademik.ArasdirmaSaheleri.Include(x=>x.Kafedra).FirstOrDefault(x => x.Id == r_id));

        }

        [HttpGet("Add")]
        public JsonResult Add()
        {
            return Json(aztuAkademik.Kafedralar);
        }


        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Post(ArasdirmaSaheleri research_area)
        {
            research_area.ArasdirmaciId = User_Id;
            await aztuAkademik.ArasdirmaSaheleri.AddAsync(research_area);
            await aztuAkademik.SaveChangesAsync();

            return Json(new { res = 1 });
        }


        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Put(ArasdirmaSaheleri research_area)
        {
            if (ModelState.IsValid)
            {
                research_area.ArasdirmaciId = User_Id;
                aztuAkademik.ArasdirmaSaheleri.Update(research_area);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }


        [HttpDelete]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var research_area = aztuAkademik.ArasdirmaSaheleri.FirstOrDefaultAsync(x => x.Id == id && x.ArasdirmaciId == User_Id);
            if (research_area.Result != null)
            {
                aztuAkademik.ArasdirmaSaheleri.Remove(research_area.Result);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }
    }
}
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
    public class PatentsController : Controller
    {

        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet]
        public JsonResult GetAll(int id) => Json(aztuAkademik.Patentler.Where(x => x.ArasdirmaciId == id));

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var patent = aztuAkademik.Patentler.FirstOrDefault(x => x.Id == id);
            return Json(patent);
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Post(Patentler patent)
        {
            patent.ArasdirmaciId = User_Id;
            await aztuAkademik.Patentler.AddAsync(patent);
            await aztuAkademik.SaveChangesAsync();

            return Json(new { res = 1 });
        }


        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Put(Patentler patent)
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

        [HttpDelete]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Delete(int id)
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
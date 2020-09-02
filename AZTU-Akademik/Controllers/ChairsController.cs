using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AZTU_Akademik.Models;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ChairsController : Controller
    {

        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        [HttpGet]
        public JsonResult GetAll() => Json(aztuAkademik.Kafedralar);

        [HttpGet("{id}")]
        public JsonResult Get(int id) => Json(aztuAkademik.Kafedralar.FirstOrDefault(x => x.Id == id));


        [HttpPost]
        public async Task<JsonResult> Post(Kafedralar chair)
        {
            await aztuAkademik.Kafedralar.AddAsync(chair);
            await aztuAkademik.SaveChangesAsync();
            return Json(new { res = 1 });
        }

        [HttpPut]
        public async Task<JsonResult> Put(Kafedralar chair)
        {
            if (ModelState.IsValid)
            {
                aztuAkademik.Kafedralar.Update(chair);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }
                return Json(new { res = 0 });
        }


        [HttpDelete]
        public async Task Delete(int id)
        {
            var chair = aztuAkademik.Kafedralar.FirstOrDefault(x => x.Id == id);
            aztuAkademik.Kafedralar.Remove(chair);
            await aztuAkademik.SaveChangesAsync();
        }


    }
}
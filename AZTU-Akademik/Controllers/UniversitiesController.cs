using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class UniversitiesController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        [HttpGet]
        public JsonResult Get() => Json(aztuAkademik.Universitetler);


        [HttpPost]
        public async Task<JsonResult> Post(Universitetler university)
        {
            await aztuAkademik.Universitetler.AddAsync(university);
            await aztuAkademik.SaveChangesAsync();
            return Json(new { res = 1 });
        }

        [HttpPut]
        public async Task<JsonResult> Put(Universitetler university)
        {
            if (ModelState.IsValid)
            {
                aztuAkademik.Universitetler.Update(university);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }
            return Json(new { res = 0 });
        }


        [HttpDelete]
        public async Task Delete(int id)
        {
            var university = aztuAkademik.Universitetler.FirstOrDefault(x => x.Id == id);
            aztuAkademik.Universitetler.Remove(university);
            await aztuAkademik.SaveChangesAsync();
        }
    }
}
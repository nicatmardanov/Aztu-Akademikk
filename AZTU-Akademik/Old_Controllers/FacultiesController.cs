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
    public class FacultiesController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        [HttpGet]
        public JsonResult GetAll() => Json(aztuAkademik.Fakulteler);

        [HttpGet("{id}")]
        public JsonResult Get(int id) => Json(aztuAkademik.Fakulteler.FirstOrDefault(x=>x.Id==id));


        [HttpPost]
        public async Task<JsonResult> Post(Fakulteler faculty)
        {
            await aztuAkademik.Fakulteler.AddAsync(faculty);
            await aztuAkademik.SaveChangesAsync();
            return Json(new { res = 1 });
        }

        [HttpPut]
        public async Task<JsonResult> Put(Fakulteler faculty)
        {
            if (ModelState.IsValid)
            {
                aztuAkademik.Fakulteler.Update(faculty);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }
            return Json(new { res = 0 });
        }


        [HttpDelete]
        public async Task Delete(int id)
        {
            var faculty = aztuAkademik.Fakulteler.FirstOrDefault(x => x.Id == id);
            aztuAkademik.Fakulteler.Remove(faculty);
            await aztuAkademik.SaveChangesAsync();
        }
    }
}
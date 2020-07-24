using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScientificActivitiesController : Controller
    {
        private readonly AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);


        [HttpGet]
        public JsonResult GetAll(int id) =>
            Json(aztuAkademik.TehsilSeviyye.Where(x => x.ArasdirmaciId == id).Include(x => x.Bakalavr).Include(x => x.Magistr).Include(x => x.ElmlerDoktoru).Include(x => x.ElmlerNamizedi).Include(x => x.Arasdirmaci));


        [HttpGet("{id}")]
        public JsonResult Get(int id) =>
            Json(aztuAkademik.TehsilSeviyye.FirstOrDefaultAsync(x => x.ArasdirmaciId == id).Result);


        [HttpGet("Add")]
        public JsonResult Add() =>
            Json(new { bachelor = aztuAkademik.BakalavriatSiyahi, master = aztuAkademik.MagistranturaSiyahisi, doctor_of_science = aztuAkademik.ElmlerDoktorluguSiyahi, cand_of_science = aztuAkademik.ElmlerNamizedlikSiyahi });


        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Post([FromQuery] TehsilSeviyye e_level/*, [FromQuery] List<int> researchers*/)
        {
            //var all_researchers = new List<int>
            //{
            //    User_Id
            //};

            //all_researchers = all_researchers.Concat(researchers).ToList();

            //TehsilSeviyye education_level;

            //for (int i = 0; i < all_researchers.Count; i++)
            //{
            //education_level = new TehsilSeviyye
            //{
            //    BakalavrId = e_level.BakalavrId,
            //    MagistrId = e_level.MagistrId,
            //    ElmlerDoktoru = e_level.ElmlerDoktoru,
            //    ElmlerNamizediId = e_level.ElmlerNamizediId,
            //    ArasdirmaciId = all_researchers[i]
            //};




            e_level.ArasdirmaciId = User_Id;
            await aztuAkademik.TehsilSeviyye.AddAsync(e_level);
            await aztuAkademik.SaveChangesAsync();
            //}

            return Json(new { res = 1 });
        }

        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Put([FromQuery] TehsilSeviyye e_level/*, [FromQuery] List<int> researchers*/)
        {
            if (ModelState.IsValid)
            {
                //var posted_all_researchers = new List<int>
                //{
                //    User_Id
                //};

                //posted_all_researchers = posted_all_researchers.Concat(researchers).ToList();



                //var get_all_researchers = aztuAkademik.TehsilSeviyye.Where(x => x.);


                //var deleted_researchers = get_all_researchers.Where(x => !posted_all_researchers.Contains((int)x.ArasdirmaciId));



                e_level.ArasdirmaciId = User_Id;


                aztuAkademik.TehsilSeviyye.Update(e_level);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }
            return Json(new { res = 0 });
        }

        [HttpDelete]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var e_level = await aztuAkademik.TehsilSeviyye.FirstOrDefaultAsync(x => x.Id == id && x.ArasdirmaciId == User_Id);
            if (e_level != null)
            {
                aztuAkademik.TehsilSeviyye.Remove(e_level);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });

            }

            return Json(new { res = 0 });

        }

    }
}
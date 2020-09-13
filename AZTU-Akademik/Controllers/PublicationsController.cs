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
    public class PublicationsController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => 1005;

        [HttpGet]
        public JsonResult GetAll(int id)
        {
            var researcher_publications = aztuAkademik.ArasdirmaciMeqale.Where(x => x.ArasdirmaciId == id);
            return Json(researcher_publications.Include(x => x.Meqale).ThenInclude(x => x.Universitet)
                .Include(x=>x.Meqale).ThenInclude(x=>x.MeqaleNov)
                .Include(x => x.Meqale).ThenInclude(x => x.MeqaleJurnal).Select(x => x.Meqale));
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            return Json(await aztuAkademik.Meqaleler.Include(x => x.Universitet).Include(x => x.MeqaleJurnal).FirstOrDefaultAsync(x => x.Id == id));
        }

        [HttpGet("Add")]
        public JsonResult Add()
        {
            return Json(aztuAkademik.Arasdirmacilar);
        }


        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Post([FromQuery] Meqaleler paper, [FromQuery] List<int> researchers)
        {
            await aztuAkademik.Meqaleler.AddAsync(paper);
            await aztuAkademik.SaveChangesAsync();


            var all_researchers = new List<int>
            {
                User_Id
            };

            all_researchers = all_researchers.Concat(researchers).ToList();


            ArasdirmaciMeqale researcher_publication;

            for (int i = 0; i < researchers.Count; i++)
            {
                researcher_publication = new ArasdirmaciMeqale
                {
                    ArasdirmaciId = researchers[i],
                    MeqaleId = paper.Id
                };
                await aztuAkademik.ArasdirmaciMeqale.AddAsync(researcher_publication);
                await aztuAkademik.SaveChangesAsync();
            }



            return Json(new { res = 1 });
        }

        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Put([FromQuery]Meqaleler paper, [FromQuery]List<int> researchers) ////////////////////////////////////
        {
            if (ModelState.IsValid)
            {
                aztuAkademik.Meqaleler.Update(paper);
                await aztuAkademik.SaveChangesAsync();


                var posted_all_researchers = new List<int>
                {
                    User_Id
                };

                posted_all_researchers = posted_all_researchers.Concat(researchers).ToList();



                var get_all_researchers = aztuAkademik.ArasdirmaciMeqale.Where(x => x.MeqaleId == paper.Id);


                var deleted_researchers = get_all_researchers.Where(x => !posted_all_researchers.Contains((int)x.ArasdirmaciId));
                aztuAkademik.ArasdirmaciMeqale.RemoveRange(deleted_researchers);
                await aztuAkademik.SaveChangesAsync();

                posted_all_researchers = posted_all_researchers.Where(x=>!get_all_researchers.Select(x=>x.ArasdirmaciId).Contains(x)).ToList();

                ArasdirmaciMeqale researcher_publication;
                for (int i = 0; i < posted_all_researchers.Count; i++)
                {
                    researcher_publication = new ArasdirmaciMeqale
                    {
                        ArasdirmaciId = posted_all_researchers[i],
                        MeqaleId = paper.Id
                    };
                    await aztuAkademik.ArasdirmaciMeqale.AddAsync(researcher_publication);
                    await aztuAkademik.SaveChangesAsync();
                }


                return Json(new { res = 1 });
            }
            return Json(new { res = 0 });
        }

        [HttpDelete]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var paper = await aztuAkademik.Meqaleler.FirstOrDefaultAsync(x => x.Id == id);
            var researcher_paper = paper.ArasdirmaciMeqale;

            if (researcher_paper.Where(x => x.ArasdirmaciId != User_Id) == null)
            {
                aztuAkademik.Meqaleler.Remove(paper);
                aztuAkademik.ArasdirmaciMeqale.RemoveRange(researcher_paper);

                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }
            return Json(new { res = 0 });
        }

    }
}
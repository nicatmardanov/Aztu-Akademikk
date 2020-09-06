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
    public class PersonalInformationController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);


        [HttpGet]
        public JsonResult Get()
        {
            var researcher = aztuAkademik.Arasdirmacilar.Include(x=>x.Kafedra).FirstOrDefaultAsync(x => x.Id == User_Id).Result;

            return Json(new { fullName = User.Claims.FirstOrDefault(x => x.Type == "FullName").Value, chair = researcher.Kafedra.KafedraAd, /*company = researcher.IsTecrubesi.Count > 0 ? researcher.IsTecrubesi.LastOrDefault().IsYeri : "",*/ email = researcher.ArasdirmaciEmeil, cv = researcher.CvAdres, profilepic = researcher.ProfilShekil });
        }


        [HttpPost("AddInformation")]
        public async Task<JsonResult> AddInformation([FromQuery] int chair_id)
        {
            var researcher = aztuAkademik.Arasdirmacilar.FirstOrDefault(x => x.Id == User_Id);



            if (Request.ContentLength>0 && Request.ContentLength>0 && Request.Form.Files.Count > 0)
            {
                researcher.CvAdres = await Classes.FileSave.Save(Request.Form.Files[0], 1);
            }

            researcher.KafedraId = chair_id;

            await aztuAkademik.SaveChangesAsync();

            return Json(new { res = 1 });

        }

        [HttpPost("AddProfilePicture")]
        public async Task<JsonResult> AddProfilePicture()
        {
            var researcher = await aztuAkademik.Arasdirmacilar.FirstOrDefaultAsync(x => x.Id == User_Id);

            if (Request.ContentLength>0 && Request.Form.Files.Count > 0)
            {
                researcher.ProfilShekil = await Classes.FileSave.Save(Request.Form.Files[0], 2);
                await aztuAkademik.SaveChangesAsync();
            }

            return Json(new { res = 1 });
        }

        [HttpPut("UpdateInformation")]
        public async Task<JsonResult> UpdateInformation([FromQuery] int chair_id, [FromQuery] bool fileChange)
        {
            var researcher = await aztuAkademik.Arasdirmacilar.FirstOrDefaultAsync(x => x.Id == User_Id);

            researcher.KafedraId = chair_id;

            if (fileChange)
            {
                if (Request.ContentLength>0 && Request.Form.Files.Count > 0)
                {
                    System.IO.File.Delete(researcher.CvAdres[1..]);
                    researcher.CvAdres = await Classes.FileSave.Save(Request.Form.Files[0], 1);
                }

            }

            await aztuAkademik.SaveChangesAsync();

            return Json(new { res = 1 });
        }


        [HttpPut("UpdateProfilePicture")]
        public async Task<JsonResult> UpdateProfilePicture()
        {
            var researcher = await aztuAkademik.Arasdirmacilar.FirstOrDefaultAsync(x => x.Id == User_Id);

            if (Request.ContentLength>0 && Request.Form.Files.Count > 0)
            {
                System.IO.File.Delete(researcher.ProfilShekil[1..]);
                researcher.ProfilShekil = await Classes.FileSave.Save(Request.Form.Files[0], 2);
                await aztuAkademik.SaveChangesAsync();
            }

            return Json(new { res = 1 });
        }
    }

}

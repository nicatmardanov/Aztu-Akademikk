using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AZTU_Akademik.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : Controller
    {

       
        readonly AztuAkademikContext aztuAkademik = new AztuAkademikContext();


        //GET

        [HttpGet]
        public JsonResult Get()
        {
            return Json(new { kafedra = aztuAkademik.Kafedralar.Select(x => new { x.Id, x.KafedraAd }).ToList(), education_level = aztuAkademik.TehsilSeviyye.Select(x => new { x.Id, x.BakalavrId, x.MagistrId, x.ElmlerNamizediId, x.ElmlerDoktoru }).ToList(), prof_admin_experience = aztuAkademik.MeslekiIdariDeneyim.Select(x => new { x.Id, x.MeslekiIdariDeneyimAd }).ToList(), pedagogic_name = aztuAkademik.ArasdirmaciPedoqojiAd.Select(x => new { x.ArasdirmaciPedoqojiAdId, x.ArasdirmaciAd }).ToList() });
        }


        //education_level = aztuAkademik.TehsilSeviyye.ToList(), prof_admin_experience = aztuAkademik.MeslekiIdariDeneyim.ToList(), pedagogic_name = aztuAkademik.ArasdirmaciPedoqojiAd.ToList()})


        //POST

        [HttpPost]
        public async Task<JsonResult> Post(Arasdirmacilar _user)
        {

            if (!_user.ArasdirmaciEmeil.Contains("@aztu.edu.az"))
                return Json(new { res = 0 });


            _user.RolId = 2;


            await aztuAkademik.Arasdirmacilar.AddAsync(_user);
            await aztuAkademik.SaveChangesAsync();
            return Json(new { res = 1 } );
        }

    }
}
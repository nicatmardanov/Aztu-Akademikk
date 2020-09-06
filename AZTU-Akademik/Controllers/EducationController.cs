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
    public class EducationController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet]
        public JsonResult GetAll()
        {

            List<string> dissertation_list = new List<string>();

            var dos_m = aztuAkademik.TehsilSeviyye.Where(x => x.ArasdirmaciId == User_Id).Include(x => x.ElmlerDoktoruNavigation).Select(x => x.ElmlerDoktoruNavigation.ElmlerDoktoruDisertasiyaAd);
            var eos_m = aztuAkademik.TehsilSeviyye.Where(x => x.ArasdirmaciId == User_Id).Include(x => x.ElmlerNamizedi).Select(x => x.ElmlerNamizedi.ElmlerNamizediDisertasiyaAd);
            var master_m = aztuAkademik.TehsilSeviyye.Where(x => x.ArasdirmaciId == User_Id).Include(x => x.Magistr).Select(x => x.Magistr.MagistrDisertasiyaAd);
           

            return Json(new
            {
                edu = aztuAkademik.TehsilSeviyye.Where(x => x.ArasdirmaciId == User_Id).Include(x => x.Bakalavr).Include(x => x.Magistr).Include(x => x.ElmlerNamizedi).Include(x => x.ElmlerDoktoruNavigation).Include(x => x.Arasdirmaci),
                dos_dissertasion = dos_m,
                eos_dissertation= eos_m,
                master_dissertation=master_m,
                foreign_language = aztuAkademik.ArasdirmaciDil.Where(x => x.ArasdirmaciId == User_Id).Select(x => new { x.XariciDil.Ad, x.DilSeviyyeNavigation.SeviyyeAd }),
                sertifikat = aztuAkademik.Sertifikatlar.Where(x => x.ArasdirmaciId == User_Id)
            });
        }
    }
}
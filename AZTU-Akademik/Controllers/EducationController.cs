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
        public JsonResult GetAll(int id) =>
            Json(new { edu_info = aztuAkademik.TehsilSeviyye.Where(x => x.ArasdirmaciId == id).Include(x => x.Bakalavr).Include(x => x.Magistr).Include(x => x.ElmlerDoktoru).Include(x => x.ElmlerNamizedi).Include(x => x.Arasdirmaci),
                       dissertasion=aztuAkademik.ArasdirmaciMeqale.Where(x=>x.ArasdirmaciId==id).Select(x=>x.Arasdirmaci),
                        sertifikat=aztuAkademik.Sertifikatlar.Where(x=>x.ArasdirmaciId==id)
            });



    }
}
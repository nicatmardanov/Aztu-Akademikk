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
    public class HomeController : Controller
    {

        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        [HttpGet]
        public JsonResult Get()
        {

            Dictionary<string, int> Count = new Dictionary<string, int>
            {
                { "PublicationCount", aztuAkademik.Meqaleler.Count() },
                { "PatentCount", aztuAkademik.Patentler.Count() },
                { "Researcher", aztuAkademik.Arasdirmacilar.Count() }
            };
            //Count.Add("Researcher", aztuAkademik.Arasdirmacilar.Count()); sitat
            //Count.Add("Researcher", aztuAkademik.Arasdirmacilar.Count()); mukafat






            return Json(new {count=Count, publications=aztuAkademik.ArasdirmaciMeqale.Include(x=>x.Arasdirmaci).Include(x=>x.Meqale), announcement=aztuAkademik.Elanlar });
        }
    }
}
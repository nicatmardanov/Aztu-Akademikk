using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        //private int Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);


        [HttpGet]
        public JsonResult Get(int id) => Json(new { email = aztuAkademik.Arasdirmacilar.FirstOrDefault(x => x.Id == id).ArasdirmaciEmeil });

    }
}
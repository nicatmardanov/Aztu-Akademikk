using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class JournalController : Controller
    {
        readonly private AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        //GET
        [HttpGet]
        public JsonResult Get(int id) => Json(aztuAkademik.Journal.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllJournals")]
        public JsonResult AllJournals() => Json(aztuAkademik.Journal.Where(x => !x.DeleteDate.HasValue));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class JournalController : Controller
    {
        public class JournalModel
        {
            public bool Indexed { get; set; }
            public string Query { get; set; }
        }
        readonly private AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        //GET
        [HttpGet]
        public JsonResult Get([FromQuery]JournalModel journalModel) =>
            string.IsNullOrEmpty(journalModel.Query) ?
            Json(aztuAkademik.Journal.Where(x => x.Indexed == journalModel.Indexed && !x.DeleteDate.HasValue).
                AsNoTracking().
                Select(x => new
                {
                    x.Id,
                    x.Name
                })) :
            Json((aztuAkademik.Journal.Where(x => x.Name.Contains(journalModel.Query) && x.Indexed == journalModel.Indexed && !x.DeleteDate.HasValue).
                AsNoTracking().
                Select(x => new
                {
                    x.Id,
                    x.Name
                })));
    }
}

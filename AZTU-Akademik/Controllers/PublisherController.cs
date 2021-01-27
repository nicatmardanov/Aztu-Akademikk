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
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class PublisherController : Controller
    {
        readonly private AztuAkademikContext aztuAkademik = new AztuAkademikContext();


        [HttpGet]
        public JsonResult Get(int id)
        {
            Publisher publisher = aztuAkademik.Publisher.FirstOrDefault(x => x.Id == id);
            return Json(new
            {
                publisher.Id,
                publisher.Name
            });
        }

        [HttpGet("AllPublishers")]
        public JsonResult AllPublishers(string query) =>
            string.IsNullOrEmpty(query) ?
            Json(aztuAkademik.Publisher.Where(x => !x.DeleteDate.HasValue).AsNoTracking().Select(x => new
            {
                x.Id,
                x.Name
            })) :
            Json(aztuAkademik.Publisher.Where(x => x.Name.Contains(query) && !x.DeleteDate.HasValue).AsNoTracking().Select(x => new
            {
                x.Id,
                x.Name
            }));

    }
}
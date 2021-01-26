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
    public class PublisherController : Controller
    {
        private DateTime GetDate
        {
            get
            {
                return DateTime.UtcNow.AddHours(4);
            }
        }
        private int User_Id
        {
            get
            {
                return int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            }
        }
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
        public JsonResult AllPublishers(int id) => Json(aztuAkademik.Publisher.Where(x => x.Id == id && !x.DeleteDate.HasValue).Select(x=>new
        {
            x.Id,
            x.Name
        }));
    }
}
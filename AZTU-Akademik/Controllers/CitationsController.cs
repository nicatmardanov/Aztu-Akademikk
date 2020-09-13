using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitationsController : Controller
    {

        [HttpGet]
        public JsonResult Get()
        {
            using (WebClient client = new WebClient())
            {
                string htmlCode = client.DownloadString("https://www.scopus.com/authid/detail.uri?authorId=7101770439&token=ee951ba52d21a688e679520b6dc21819");
                return Json(htmlCode);
            }
        }
    }
}
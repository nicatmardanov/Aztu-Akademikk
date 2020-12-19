using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementsController : Controller
    {
        readonly private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
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


        //GET
        [HttpGet]
        public JsonResult Get(int user_id)
        {
            using (WebClient client = new WebClient())
            {


                var indexOf = client.DownloadString("https://scholar.google.com/citations?user=UWPeriIAAAAJ&hl=en&oi=ao").IndexOf("gsc_rsb_st");

                System.Text.StringBuilder htmlCode = new System.Text.StringBuilder();

                


                
                return Json(client.DownloadString("https://google.com"));
            }



        }


    }
}
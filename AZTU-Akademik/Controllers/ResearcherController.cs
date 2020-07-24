using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AZTU_Akademik.Models;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ResearcherController : Controller
    {
        readonly AztuAkademikContext aztuAkademik = new AztuAkademikContext();


        //PUT

        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Put(Arasdirmacilar _user)
        {
            if (!string.IsNullOrEmpty(_user.ArasdirmaciEmeil) && !_user.ArasdirmaciEmeil.Contains("@aztu.edu.az"))
                return Json(new { res = "false" });

            if (ModelState.IsValid)
            {
                aztuAkademik.Arasdirmacilar.Update(_user);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = "true" });
            }

            return Json(new { res = "false" });
        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class CertificatesController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        [HttpGet]
        public JsonResult Get() => Json(aztuAkademik.Sertifikatlar);


        [HttpPost]
        public async Task<JsonResult> Post(Sertifikatlar certificate)
        {
            await aztuAkademik.Sertifikatlar.AddAsync(certificate);
            await aztuAkademik.SaveChangesAsync();
            return Json(new { res = 1 });
        }

        [HttpPut]
        public async Task<JsonResult> Put(Sertifikatlar certificate)
        {
            if (ModelState.IsValid)
            {
                aztuAkademik.Sertifikatlar.Update(certificate);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }
            return Json(new { res = 0 });
        }


        [HttpDelete]
        public async Task Delete(int id)
        {
            var certificate = aztuAkademik.Sertifikatlar.FirstOrDefault(x => x.Id == id);
            aztuAkademik.Sertifikatlar.Remove(certificate);
            await aztuAkademik.SaveChangesAsync();
        }
    }
}
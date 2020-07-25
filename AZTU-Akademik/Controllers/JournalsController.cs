﻿using System;
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
    public class JournalsController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        [HttpGet]
        public JsonResult Get() => Json(aztuAkademik.Jurnallar);


        [HttpPost]
        public async Task<JsonResult> Post(Jurnallar journal)
        {
            await aztuAkademik.Jurnallar.AddAsync(journal);
            await aztuAkademik.SaveChangesAsync();
            return Json(new { res = 1 });
        }

        [HttpPut]
        public async Task<JsonResult> Put(Jurnallar journal)
        {
            if (ModelState.IsValid)
            {
                aztuAkademik.Jurnallar.Update(journal);
                await aztuAkademik.SaveChangesAsync();
                return Json(new { res = 1 });
            }
            return Json(new { res = 0 });
        }


        [HttpDelete]
        public async Task Delete(int id)
        {
            var journal = aztuAkademik.Jurnallar.FirstOrDefault(x => x.Id == id);
            aztuAkademik.Jurnallar.Remove(journal);
            await aztuAkademik.SaveChangesAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ForeignLanguagesController : Controller
    {
        private readonly AztuAkademikContext aztuAkademik = new AztuAkademikContext();


        [HttpGet("AllForeignLanguagesList")]
        public JsonResult GetAll() => Json(aztuAkademik.XariciDil);


        [HttpGet("{id}")]
        public JsonResult Get(int id) => Json(aztuAkademik.ArasdirmaciDil.Where(x => x.ArasdirmaciId == id).Include(x => x.XariciDil));


        [HttpPost("AddLanguage")]
        public async Task AddLanguage(XariciDil f_lang)
        {
            await aztuAkademik.XariciDil.AddAsync(f_lang);
            await aztuAkademik.SaveChangesAsync();
        }

        [HttpPost("AddLanguageResearcher")]
        public async Task AddLanguageResearcher(ArasdirmaciDil r_lang)
        {
            await aztuAkademik.ArasdirmaciDil.AddAsync(r_lang);
            await aztuAkademik.SaveChangesAsync();
        }


        [HttpPut("UpdateLanguage")]
        public async Task UpdateLanguage(XariciDil f_lang)
        {
            aztuAkademik.XariciDil.Update(f_lang);
            await aztuAkademik.SaveChangesAsync();
        }


        [HttpPut("UpdateLanguageResearcher")]
        public async Task UpdateLanguageResearcher(ArasdirmaciDil r_lang)
        {
            aztuAkademik.ArasdirmaciDil.Update(r_lang);
            await aztuAkademik.SaveChangesAsync();
        }


        [HttpDelete("DeleteLanguage")]
        public async Task DeleteLanguage(int id)
        {
            var lang = await aztuAkademik.XariciDil.FirstOrDefaultAsync(x => x.Id == id);
            aztuAkademik.XariciDil.Remove(lang);
            await aztuAkademik.SaveChangesAsync();
        }


        [HttpDelete("DeleteLanguageResearcher")]
        public async Task DeleteLanguageResearcher(ArasdirmaciDil r_lang)
        {
            aztuAkademik.ArasdirmaciDil.Remove(r_lang);
            await aztuAkademik.SaveChangesAsync();
        }


    }
}
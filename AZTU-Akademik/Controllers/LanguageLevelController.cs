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
    public class LanguageLevelController : Controller
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
        [HttpGet("Level")]
        public JsonResult Level(short id) => Json(aztuAkademik.LanguageLevels.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllLevels")]
        public JsonResult AllLevels() => Json(aztuAkademik.LanguageLevels.Where(x => !x.DeleteDate.HasValue));


        //POST
        [HttpPost]
        public async Task Post(LanguageLevels _languageLevels)
        {
            _languageLevels.CreateDate = GetDate;

            await aztuAkademik.LanguageLevels.AddAsync(_languageLevels);
            await aztuAkademik.SaveChangesAsync();
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(LanguageLevels _languageLevels)
        {
            if (ModelState.IsValid)
            {
                _languageLevels.UpdateDate = GetDate;
                aztuAkademik.Attach(_languageLevels);
                aztuAkademik.Entry(_languageLevels).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_languageLevels).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
                
                return 1;
            }
            return 0;
        }


        //Delete
        [HttpDelete]
        public async Task Delete(short id)
        {
            aztuAkademik.LanguageLevels.FirstOrDefault(x => x.Id == id).DeleteDate = GetDate;
            aztuAkademik.LanguageLevels.FirstOrDefault(x => x.Id == id).StatusId = 0;
            await aztuAkademik.SaveChangesAsync();
        }

    }
}
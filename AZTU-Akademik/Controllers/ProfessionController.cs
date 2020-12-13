using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfessionController : Controller
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
        [HttpGet("Profession")]
        [AllowAnonymous]
        public JsonResult Profession(int id) => Json(aztuAkademik.Profession.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllProfessions")]
        [AllowAnonymous]
        public JsonResult AllProfessions() => Json(aztuAkademik.Profession.Where(x => !x.DeleteDate.HasValue));

        //POST
        [HttpPost]
        public async Task Post(Profession _profession)
        {
            _profession.CreateDate = GetDate;

            await aztuAkademik.Profession.AddAsync(_profession);
            await aztuAkademik.SaveChangesAsync();
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(Profession _profession)
        {
            if (ModelState.IsValid)
            {
                _profession.UpdateDate = GetDate;
                aztuAkademik.Attach(_profession);
                aztuAkademik.Entry(_profession).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_profession).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.Profession.FirstOrDefault(x => x.Id == id).DeleteDate = GetDate;
            aztuAkademik.Profession.FirstOrDefault(x => x.Id == id).StatusId = 0;
            await aztuAkademik.SaveChangesAsync();
        }
    }
}
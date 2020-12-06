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
    public class ResearchAreaController : Controller
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
        [HttpGet("ResearchArea")]
        public JsonResult ResearchArea(int id) => Json(aztuAkademik.ResearchArea.FirstOrDefault(x => x.Id == id));

        [HttpGet("AllResearchAreas")]
        public JsonResult AllResearchAreas() => Json(aztuAkademik.ResearchArea);


        //POST
        [HttpPost]
        public async Task Post(List<ResearchArea> _researchArea)
        {

            _researchArea.ForEach(x => x.CreateDate = GetDate);
            await aztuAkademik.ResearchArea.AddRangeAsync(_researchArea);
            await aztuAkademik.SaveChangesAsync();
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(ResearchArea _researchArea)
        {
            if (ModelState.IsValid)
            {
                _researchArea.UpdateDate = GetDate;
                aztuAkademik.Entry(_researchArea).State = EntityState.Modified;
                aztuAkademik.Entry(_researchArea).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
                await aztuAkademik.SaveChangesAsync();

                return 1;
            }
            return 0;
        }

        //Delete
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.ResearchArea.FirstOrDefaultAsync(x => x.Id == id).Result.DeleteDate = GetDate;
            aztuAkademik.ResearchArea.FirstOrDefaultAsync(x => x.Id == id).Result.StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
        }

    }
}
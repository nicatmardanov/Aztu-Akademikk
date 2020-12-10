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
    public class ResearcherDegreeController : Controller
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
        [HttpGet("DegreeForUser")]
        public JsonResult DegreeForUser(int user_id) => Json(aztuAkademik.RelResearcherDegree.Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue).
            Include(x => x.Degree).Include(x => x.Researcher));

        //POST
        [HttpPost]
        public async Task Post(RelResearcherDegree _relResearcherDegree)
        {
            _relResearcherDegree.CreateDate = GetDate;

            await aztuAkademik.RelResearcherDegree.AddAsync(_relResearcherDegree);
            await aztuAkademik.SaveChangesAsync();
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(RelResearcherDegree _relResearcherDegree)
        {
            if (ModelState.IsValid)
            {
                _relResearcherDegree.UpdateDate = GetDate;
                aztuAkademik.Attach(_relResearcherDegree);
                aztuAkademik.Entry(_relResearcherDegree).State = EntityState.Modified;
                aztuAkademik.Entry(_relResearcherDegree).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_relResearcherDegree).Property(x => x.ResearcherId).IsModified = false;


                await aztuAkademik.SaveChangesAsync();

                return 1;
            }
            return 0;
        }

        //Delete
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.RelResearcherDegree.FirstOrDefaultAsync(x => x.Id == id).Result.DeleteDate = GetDate;
            aztuAkademik.RelResearcherDegree.FirstOrDefaultAsync(x => x.Id == id).Result.StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
        }

    }
}
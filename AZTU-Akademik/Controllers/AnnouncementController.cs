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
    public class AnnouncementController : Controller
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
        [HttpGet("Announcements")]
        [AllowAnonymous]
        public JsonResult Announcements(int user_id) => Json(aztuAkademik.Announcement.Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue));


        //POST
        [HttpPost]
        public async Task Post(Announcement _announcement)
        {
            _announcement.CreateDate = GetDate;
            _announcement.ResearcherId = User_Id;
            await aztuAkademik.Announcement.AddAsync(_announcement);
            await aztuAkademik.SaveChangesAsync();
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(Announcement _announcement)
        {
            if (ModelState.IsValid)
            {
                _announcement.UpdateDate = GetDate;
                aztuAkademik.Attach(_announcement);
                aztuAkademik.Entry(_announcement).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_announcement).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_announcement).Property(x => x.ResearcherId).IsModified = false;

                await aztuAkademik.SaveChangesAsync();

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(long id)
        {
            aztuAkademik.Announcement.FirstOrDefault(x => x.Id == id).DeleteDate = GetDate;
            aztuAkademik.Announcement.FirstOrDefault(x => x.Id == id).StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
        }



    }
}
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
    public class ContactController : Controller
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
        [HttpGet("Contact")]
        public JsonResult Contact(int user_id) => Json(aztuAkademik.Contact.Where(x => x.ResearcherId == user_id).Include(x => x.Type));


        //POST
        [HttpPost]
        public async Task Post(List<Contact> _contact)
        {
            _contact.ForEach(x =>
            {
                x.CreateDate = GetDate;
                x.ResearcherId = User_Id;
            });

            await aztuAkademik.Contact.AddRangeAsync(_contact);
            await aztuAkademik.SaveChangesAsync();
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(Contact _contact)
        {
            if(ModelState.IsValid)
            {
                _contact.UpdateDate = GetDate;
                aztuAkademik.Entry(_contact).State = EntityState.Modified;
                aztuAkademik.Entry(_contact).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
                await aztuAkademik.SaveChangesAsync();

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.Contact.FirstOrDefaultAsync(x => x.Id == id).Result.DeleteDate = GetDate;
            aztuAkademik.Contact.FirstOrDefaultAsync(x => x.Id == id).Result.StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
        }

    }
}
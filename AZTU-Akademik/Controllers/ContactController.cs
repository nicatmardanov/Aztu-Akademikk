using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        private string IpAdress { get; }
        private string AInformation { get; }

        public ContactController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Contact(int user_id) => Json(aztuAkademik.Contact.Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Type).AsNoTracking());


        //POST
        [HttpPost]
        public async Task Post(List<Contact> _contact)
        {
            _contact.ForEach(x =>
            {
                x.CreateDate = GetDate;
                x.ResearcherId = User_Id;
            });

            await aztuAkademik.Contact.AddRangeAsync(_contact).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Contact", "", _contact.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(Contact _contact)
        {
            if (ModelState.IsValid)
            {
                _contact.UpdateDate = GetDate;
                aztuAkademik.Entry(_contact).State = EntityState.Modified;
                aztuAkademik.Entry(_contact).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_contact).Property(x => x.ResearcherId).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Contact", "", _contact.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            Contact contact = await aztuAkademik.Contact.FirstOrDefaultAsync(x => x.Id == id && x.ResearcherId == User_Id).ConfigureAwait(false);
            contact.DeleteDate = GetDate;
            contact.StatusId = 0;

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Contact", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

    }
}
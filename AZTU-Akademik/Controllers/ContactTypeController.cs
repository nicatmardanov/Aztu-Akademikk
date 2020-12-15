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
    [Authorize(Roles = "Admin")]
    public class ContactTypeController : Controller
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

        public ContactTypeController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet("Type")]
        [AllowAnonymous]
        public JsonResult Type(short id) => Json(aztuAkademik.ContactType.FirstOrDefault(x => x.Id == id));

        //POST
        [HttpPost]
        public async Task Post(ContactType _type)
        {
            if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
                _type.Icon = await Classes.FileSave.Save(Request.Form.Files[0], 3);

            _type.CreateDate = GetDate;

            await aztuAkademik.ContactType.AddAsync(_type);
            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("ContactType", "", _type.Id, 1, User_Id, IpAdress, AInformation);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery]ContactType _type, [FromQuery] bool fileChange)
        {
            if (ModelState.IsValid)
            {
                if (fileChange)
                {
                    if (!string.IsNullOrEmpty(_type.Icon))
                        System.IO.File.Delete(_type.Icon[1..]);

                    _type.Icon = await Classes.FileSave.Save(Request.Form.Files[0], 3);

                }
                _type.UpdateDate = GetDate;
                aztuAkademik.Entry(_type).State = EntityState.Modified;
                aztuAkademik.Entry(_type).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
                await Classes.TLog.Log("ContactType", "", _type.Id, 2, User_Id, IpAdress, AInformation);
                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.ContactType.FirstOrDefaultAsync(x => x.Id == id).Result.DeleteDate = GetDate;
            aztuAkademik.ContactType.FirstOrDefaultAsync(x => x.Id == id).Result.StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("ContactType", "", id, 3, User_Id, IpAdress, AInformation);
        }


    }

}

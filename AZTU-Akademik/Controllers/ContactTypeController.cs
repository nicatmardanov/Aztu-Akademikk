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
        public class ContactTypeModel
        {
            public ContactType Type { get; set; } 
            public bool FileChange { get; set; }
        }

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
        [HttpGet]
        [AllowAnonymous]
        public JsonResult ContactType(short id) => Json(aztuAkademik.ContactType.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllContactTypes")]
        [AllowAnonymous]
        public JsonResult AllContactTypes() => Json(aztuAkademik.ContactType.AsNoTracking().Where(x => !x.DeleteDate.HasValue).OrderByDescending(x => x.Id));

        //POST
        [HttpPost]
        public async Task Post([FromForm]ContactType _type)
        {
            if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
                _type.Icon = await Classes.FileSave.Save(Request.Form.Files[0], 3).ConfigureAwait(false);

            _type.CreateDate = GetDate;

            await aztuAkademik.ContactType.AddAsync(_type).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ContactType", "", _type.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put([FromForm]ContactTypeModel contactTypeModel)
        {
            if (ModelState.IsValid)
            {
                if (contactTypeModel.FileChange)
                {
                    if (!string.IsNullOrEmpty(contactTypeModel.Type.Icon))
                        System.IO.File.Delete(contactTypeModel.Type.Icon[1..]);

                    contactTypeModel.Type.Icon = await Classes.FileSave.Save(Request.Form.Files[0], 3).ConfigureAwait(false);

                }
                contactTypeModel.Type.UpdateDate = GetDate;
                aztuAkademik.Entry(contactTypeModel.Type).State = EntityState.Modified;
                aztuAkademik.Entry(contactTypeModel.Type).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("ContactType", "", contactTypeModel.Type.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            ContactType contactType = await aztuAkademik.ContactType.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            contactType.DeleteDate = GetDate;
            contactType.StatusId = 0;

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ContactType", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }

}

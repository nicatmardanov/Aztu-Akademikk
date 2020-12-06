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


        //GET
        [HttpGet("Type")]
        public JsonResult Type(short id) => Json(aztuAkademik.ContactType.FirstOrDefault(x => x.Id == id));

        //POST
        [HttpPost]
        public async Task Post(ContactType _type)
        {
            string icon_url = await Classes.FileSave.Save(Request.Form.Files[0], 3);
            _type.Icon = icon_url;
            _type.CreateDate = GetDate;

            await aztuAkademik.ContactType.AddAsync(_type);
            await aztuAkademik.SaveChangesAsync();
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
        }


    }

}

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
    public class CertificateController : Controller
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
        [HttpGet("Certificate")]
        public JsonResult Certificate(int user_id) => Json(aztuAkademik.Certificate.
            Include(x=>x.File).Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue));


        //POST
        [HttpPost]
        public async Task Post(Certificate _certificate)
        {
            if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
            {
                File _file = new File
                {
                    Name = await Classes.FileSave.Save(Request.Form.Files[0], 2),
                    Type = 3,
                    CreateDate = GetDate,
                    StatusId = 1,
                    UserId = User_Id
                };


                await aztuAkademik.File.AddAsync(_file);
                await aztuAkademik.SaveChangesAsync();

                _certificate.CreateDate = GetDate;
                _certificate.ResearcherId = User_Id;
                _certificate.FileId = _file.Id;

                await aztuAkademik.Certificate.AddAsync(_certificate);
                await aztuAkademik.SaveChangesAsync();
            }
        }

        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery]Certificate _certificate, bool fileChange)
        {
            if (ModelState.IsValid)
            {
                if (fileChange)
                {
                    File _file = await aztuAkademik.File.FirstOrDefaultAsync(x => x.Id == _certificate.FileId);
                    if (!string.IsNullOrEmpty(_file.Name))
                        System.IO.File.Delete(_file.Name[1..]);

                    _file.Name = await Classes.FileSave.Save(Request.Form.Files[0], 2);
                    _file.UpdateDate = GetDate;
                }

                _certificate.UpdateDate = GetDate;
                aztuAkademik.Entry(_certificate).State = EntityState.Modified;
                aztuAkademik.Entry(_certificate).Property(x => x.CreateDate).IsModified = false;

                
                await aztuAkademik.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            aztuAkademik.Certificate.FirstOrDefaultAsync(x => x.Id == id).Result.DeleteDate = GetDate;
            aztuAkademik.Certificate.FirstOrDefaultAsync(x => x.Id == id).Result.StatusId = 0;
            await aztuAkademik.SaveChangesAsync();
        }
    }
}
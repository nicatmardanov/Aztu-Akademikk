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

        private string IpAdress { get; }
        private string AInformation { get; }

        public CertificateController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }



        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Certificate(int user_id) => Json(aztuAkademik.Certificate.
            Include(x => x.File).Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).AsNoTracking().
            Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.StartDate,
                x.EndDate
            }));


        //POST
        [HttpPost]
        public async Task Post([FromForm] Certificate _certificate)
        {
            if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
            {
                File _file = new File
                {
                    Name = await Classes.FileSave.Save(Request.Form.Files[0], 2).ConfigureAwait(false),
                    Type = 3,
                    CreateDate = GetDate,
                    StatusId = 1,
                    UserId = User_Id
                };


                await aztuAkademik.File.AddAsync(_file).ConfigureAwait(false);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("File", "", _file.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                _certificate.FileId = _file.Id;


            }

            _certificate.CreateDate = GetDate;
            _certificate.ResearcherId = User_Id;

            await aztuAkademik.Certificate.AddAsync(_certificate).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);

            await Classes.TLog.Log("Certificate", "", _certificate.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery]Certificate _certificate, [FromQuery] bool fileChange)
        {
            if (ModelState.IsValid)
            {
                if (fileChange)
                {
                    File _file = await aztuAkademik.File.FirstOrDefaultAsync(x => x.Id == _certificate.FileId).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(_file.Name))
                        System.IO.File.Delete(_file.Name[1..]);

                    _file.Name = await Classes.FileSave.Save(Request.Form.Files[0], 2).ConfigureAwait(false);
                    _file.UpdateDate = GetDate;
                    await Classes.TLog.Log("File", "", _file.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                }

                _certificate.UpdateDate = GetDate;
                aztuAkademik.Entry(_certificate).State = EntityState.Modified;
                aztuAkademik.Entry(_certificate).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_certificate).Property(x => x.ResearcherId).IsModified = false;


                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Certificate", "", _certificate.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                return 1;
            }
            return 0;
        }

        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            Certificate certificate = await aztuAkademik.Certificate.FirstOrDefaultAsync(x => x.Id == id && x.ResearcherId == User_Id).ConfigureAwait(false);
            certificate.DeleteDate = GetDate;
            certificate.StatusId = 0;
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Certificate", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }
    }
}
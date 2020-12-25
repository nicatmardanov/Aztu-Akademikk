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
    public class CountryController : Controller
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

        public CountryController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet("Country")]
        [AllowAnonymous]
        public JsonResult Country(short id) => Json(aztuAkademik.Country.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));


        [HttpGet("AllCountries")]
        [AllowAnonymous]
        public JsonResult AllCountries() => Json(aztuAkademik.Country.Where(x => !x.DeleteDate.HasValue).AsNoTracking());



        //POST
        [HttpPost]
        public async Task Post(Country _country)
        {
            _country.CreateDate = GetDate;
            await aztuAkademik.Country.AddAsync(_country).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Country", "", _country.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


        //PUT
        [HttpPut]
        public async Task<int> Put(Country _country)
        {
            if (ModelState.IsValid)
            {
                _country.UpdateDate = GetDate;
                aztuAkademik.Attach(_country);
                aztuAkademik.Entry(_country).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_country).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Country", "", _country.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                return 1;
            }

            return 0;
        }


        //Delete
        [HttpDelete]
        public async Task Delete(short id)
        {
            Country country = await aztuAkademik.Country.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            country.DeleteDate = GetDate;
            country.StatusId = 0;

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Country", "", id, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        //GET
        [HttpGet("Country")]
        public JsonResult Country(short id) => Json(aztuAkademik.Country.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));


        [HttpGet("AllCountries")]
        public JsonResult AllCountries() => Json(aztuAkademik.Country.Where(x => !x.DeleteDate.HasValue));



        //POST
        [HttpPost]
        public async Task Post(Country _country)
        {
            _country.CreateDate = GetDate;
            await aztuAkademik.Country.AddAsync(_country);
            await aztuAkademik.SaveChangesAsync();
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

                await aztuAkademik.SaveChangesAsync();
                return 1;
            }

            return 0;
        }


        //Delete
        [HttpDelete]
        public async Task Delete(short id)
        {
            aztuAkademik.Country.FirstOrDefault(x => x.Id == id).DeleteDate = GetDate;
            aztuAkademik.Country.FirstOrDefault(x => x.Id == id).StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
        }

    }
}
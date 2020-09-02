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
    public class ContactController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet]
        public JsonResult GetAll() => Json(aztuAkademik.Elaqe.Where(x => x.ArasdirmaciId == User_Id));

        [HttpGet("{id}")]
        public JsonResult Get(int id) => Json(aztuAkademik.Elaqe.FirstOrDefault(x=>x.ArasdirmaciId==User_Id && x.Id==id));

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Post(Elaqe contact)
        {
            contact.ArasdirmaciId = User_Id;
            await aztuAkademik.Elaqe.AddAsync(contact);
            await aztuAkademik.SaveChangesAsync();

            return Json(1);
        }

        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Update(Elaqe contact)
        {
            if (ModelState.IsValid)
            {
                contact.ArasdirmaciId = User_Id;
                aztuAkademik.Elaqe.Update(contact);
                await aztuAkademik.SaveChangesAsync();
                return Json(1);
            }

            return Json(0);
        }

        [HttpDelete]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var contact = aztuAkademik.Elaqe.FirstOrDefault(x => x.Id == id && x.ArasdirmaciId == User_Id);

            if (contact != null)
            {
                aztuAkademik.Elaqe.Remove(contact);
                await aztuAkademik.SaveChangesAsync();

                return Json(1);
            }

            return Json(0);

        }

    }
}
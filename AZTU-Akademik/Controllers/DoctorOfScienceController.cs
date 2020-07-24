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
    public class DoctorOfScienceController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);


        [HttpGet("Add")]
        public JsonResult Add() => Json(aztuAkademik.Universitetler);

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Post(ElmlerDoktorluguSiyahi d_list)
        {
            await aztuAkademik.ElmlerDoktorluguSiyahi.AddAsync(d_list);
            await aztuAkademik.SaveChangesAsync();

            return Json(d_list.Id);
        }

        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Put(ElmlerDoktorluguSiyahi d_list)
        {
            if (ModelState.IsValid)
            {
                aztuAkademik.ElmlerDoktorluguSiyahi.Update(d_list);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }

        [HttpDelete]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var d_list = aztuAkademik.ElmlerDoktorluguSiyahi.FirstOrDefault(x => x.Id == id);

            if (d_list != null)
            {
                aztuAkademik.ElmlerDoktorluguSiyahi.Remove(d_list);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });

        }
    }
}
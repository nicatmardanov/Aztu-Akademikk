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
    public class CandidateOfScienceController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);


        [HttpGet("Add")]
        public JsonResult Add() => Json(aztuAkademik.Universitetler);

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Post(ElmlerNamizedlikSiyahi c_list)
        {
            await aztuAkademik.ElmlerNamizedlikSiyahi.AddAsync(c_list);
            await aztuAkademik.SaveChangesAsync();

            return Json(c_list.Id);
        }

        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Put(ElmlerNamizedlikSiyahi c_list)
        {
            if (ModelState.IsValid)
            {
                aztuAkademik.ElmlerNamizedlikSiyahi.Update(c_list);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }

        [HttpDelete]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var c_list = aztuAkademik.ElmlerNamizedlikSiyahi.FirstOrDefault(x => x.Id == id);

            if (c_list != null)
            {
                aztuAkademik.ElmlerNamizedlikSiyahi.Remove(c_list);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });

        }
    }
}
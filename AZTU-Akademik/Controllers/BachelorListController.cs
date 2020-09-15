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
    public class BachelorListController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

        [HttpGet("GetAll")]
        public JsonResult GetAll() => Json(aztuAkademik.BakalavriatSiyahi.Include(x => x.TehsilSeviyye));

        [HttpGet("Get")]
        public JsonResult Get(int id) => Json(aztuAkademik.TehsilSeviyye.Where(x => x.ArasdirmaciId == id && x.BakalavrId > 0));


        [HttpGet("Add")]
        public JsonResult Add() => Json(aztuAkademik.Universitetler);

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Post(BakalavriatSiyahi b_list)
        {
            await aztuAkademik.BakalavriatSiyahi.AddAsync(b_list);
            await aztuAkademik.SaveChangesAsync();

            return Json(b_list.Id);
        }

        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Put(BakalavriatSiyahi b_list)
        {
            if (ModelState.IsValid)
            {
                aztuAkademik.BakalavriatSiyahi.Update(b_list);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }

        [HttpDelete]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var b_list = aztuAkademik.BakalavriatSiyahi.FirstOrDefault(x => x.Id == id);

            if (b_list != null)
            {
                b_list.TehsilSeviyye.FirstOrDefault().BakalavrId = null;
                await aztuAkademik.SaveChangesAsync();


                aztuAkademik.BakalavriatSiyahi.Remove(b_list);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });

        }

    }
}
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
    public class MasterListController : Controller
    {
        private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private int User_Id => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);


        [HttpGet("Add")]
        public JsonResult Add() => Json(aztuAkademik.Universitetler);

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Post(MagistranturaSiyahisi m_list)
        {
            await aztuAkademik.MagistranturaSiyahisi.AddAsync(m_list);
            await aztuAkademik.SaveChangesAsync();

            return Json(m_list.Id);
        }

        [HttpPut]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Put(MagistranturaSiyahisi m_list)
        {
            if (ModelState.IsValid)
            {
                aztuAkademik.MagistranturaSiyahisi.Update(m_list);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });
        }

        [HttpDelete]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var m_list = aztuAkademik.MagistranturaSiyahisi.FirstOrDefault(x => x.Id == id);

            if (m_list != null)
            {
                aztuAkademik.MagistranturaSiyahisi.Remove(m_list);
                await aztuAkademik.SaveChangesAsync();

                return Json(new { res = 1 });
            }

            return Json(new { res = 0 });

        }
    }
}
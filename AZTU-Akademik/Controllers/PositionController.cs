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
    public class PositionController : Controller
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

        public PositionController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet("Position")]
        [AllowAnonymous]
        public JsonResult Position(int id) => Json(aztuAkademik.Position.FirstOrDefault(x => x.Id == id && !x.DeleteDate.HasValue));

        [HttpGet("AllPositions")]
        [AllowAnonymous]
        public JsonResult AllPositions() => Json(aztuAkademik.Position.Where(x => !x.DeleteDate.HasValue));


        //POST
        [HttpPost]
        public async Task Post(Position _position)
        {
            _position.CreateDate = GetDate;

            await aztuAkademik.Position.AddAsync(_position);
            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("Position", "", _position.Id, 1, User_Id, IpAdress, AInformation);
        }

        //PUT
        [HttpPut]
        public async Task<int> Put(Position _position)
        {
            if (ModelState.IsValid)
            {
                _position.UpdateDate = GetDate;
                aztuAkademik.Attach(_position);
                aztuAkademik.Entry(_position).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                aztuAkademik.Entry(_position).Property(x => x.CreateDate).IsModified = false;

                await aztuAkademik.SaveChangesAsync();
                await Classes.TLog.Log("Position", "", _position.Id, 2, User_Id, IpAdress, AInformation);

                return 1;
            }
            return 0;
        }

        //DELETE
        [HttpDelete]
        public async Task Delete(int id)
        {
            Position position = await aztuAkademik.Position.FirstOrDefaultAsync(x => x.Id == id);
            position.DeleteDate = GetDate;
            position.StatusId = 0;

            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("Position", "", id, 3, User_Id, IpAdress, AInformation);
        }

    }
}
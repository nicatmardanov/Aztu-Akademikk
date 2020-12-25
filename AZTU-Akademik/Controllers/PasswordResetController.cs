using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AZTU_Akademik.Models;
using Microsoft.EntityFrameworkCore;
using AZTU_Akademik.Classes;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : Controller
    {
        readonly private AztuAkademikContext aztuAkademik = new AztuAkademikContext();
        private DateTime GetDate
        {
            get
            {
                return  DateTime.UtcNow.AddHours(4);
            }
        }

        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] string hash, [FromQuery] string email)
        {
            User user = await aztuAkademik.User.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);

            PasswordReset passwordReset = await aztuAkademik.PasswordReset.AsNoTracking().
                LastOrDefaultAsync(x => x.Hash == hash && x.UserId == user.Id && 
                GetDate.Subtract(x.CreateDate.Value).Days >= 0 && GetDate.Subtract(x.CreateDate.Value).Days < 1).ConfigureAwait(false);


            if (user == null && passwordReset == null)
                return Json(Array.Empty<byte>());



            return Json(new { UserInformation = User, PassReset = passwordReset });
        }


        [HttpPost]
        public async Task<byte> Post(string email)
        {
            User user = await aztuAkademik.User.FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
            if (user == null)
                return 0;


            aztuAkademik.PasswordReset.Add(new PasswordReset
            {
                Code = user.Password,
                Hash = HashSHA512.ComputeSha512Hash(user.Password),
                UserId = user.Id,
                CreateDate = GetDate
            });

            //Email.SendEmail();

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            return 1;
        }


        [HttpPut("PasswordChange")]
        public async Task<byte> PasswordChange([FromQuery] string hash, [FromQuery] string email, [FromQuery] string newPassword)
        {
            User user = await aztuAkademik.User.FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
            PasswordReset passwordReset = await aztuAkademik.PasswordReset.
                LastOrDefaultAsync(x => x.Hash == hash && x.UserId == user.Id && 
                GetDate.Subtract(x.CreateDate.Value).Days >= 0 && GetDate.Subtract(x.CreateDate.Value).Days < 1).ConfigureAwait(false);

            if (user == null && passwordReset == null)
                return 0;

            user.Password = newPassword;
            user.UpdateDate = GetDate;
            passwordReset.DeleteDate = GetDate;

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);

            return 1;
        }


    }




}
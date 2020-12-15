using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : Controller
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

        public SignUpController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //POST

        [HttpPost]
        public async Task<int> Post(User _user)
        {

            if (!_user.Email.Contains("@aztu.edu.az"))
                return 0;


            _user.RoleId = 0;
            _user.CreateDate = GetDate;


            await aztuAkademik.User.AddAsync(_user);
            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("User", "", _user.Id, 1, User_Id, IpAdress, AInformation);

            return 1;
        }
    }
}
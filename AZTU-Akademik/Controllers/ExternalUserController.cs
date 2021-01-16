using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AZTU_Akademik.Models;
using System.Security.Claims;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ExternalUserController : ControllerBase
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

        public ExternalUserController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //POST
        [HttpPost]
        public async Task<int> Post(ExternalResearcher _externalResearcher)
        {
            _externalResearcher.CreateDate = GetDate;
            _externalResearcher.StatusId = 1;
            await aztuAkademik.ExternalResearcher.AddAsync(_externalResearcher).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);

            await Classes.TLog.Log("ExternalResearcher", "", _externalResearcher.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

            return _externalResearcher.Id;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        [HttpGet]
        public JsonResult Get() => User.Identity.IsAuthenticated ? Json(new { isAuth = true, role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role), id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier) }) : Json(new { isAuth = false, role = false, id = false });

    }
}
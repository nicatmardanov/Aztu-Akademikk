using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZTU_Akademik.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AZTU_Akademik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalUserController : Controller
    {
        readonly private AztuAkademikContext aztuAkademik = new AztuAkademikContext();

        //GET
        [HttpGet]
        public JsonResult Get(string userName)
        {
            string[] user_array = userName.Split(' ');

            IQueryable<User> users = aztuAkademik.User.
                Where(x => x.FirstName.Contains(user_array[0]) || x.LastName.Contains(user_array[0])).
                AsNoTracking();

            if (user_array.Length == 2)
                users = users.Where(x => x.FirstName.Contains(user_array[0]) || x.LastName.Contains(user_array[0]));

            return Json(users);
        }
    }
}
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
    public class TextbookController : Controller
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


        //GET
        [HttpGet("Textbook")]
        [AllowAnonymous]
        public JsonResult Textbook(int user_id) => Json(aztuAkademik.RelTextbookResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Textbook).ThenInclude(x => x.Publisher).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x => x.Organization));



        //POST
        [HttpPost]
        public async Task Post([FromQuery] Textbook _textbook, [FromQuery] List<RelTextbookResearcher> _relTextbookResearchers)
        {
            _textbook.CreateDate = GetDate;
            _textbook.CreatorId = User_Id;
            await aztuAkademik.Textbook.AddAsync(_textbook);
            await aztuAkademik.SaveChangesAsync();


            _relTextbookResearchers.ForEach(x =>
            {
                x.CreateDate = GetDate;
                x.TextbookId = _textbook.Id;
            });


            await aztuAkademik.RelTextbookResearcher.AddRangeAsync(_relTextbookResearchers);
            await aztuAkademik.SaveChangesAsync();
        }




        //PUT
        //////////
        ///


        //DELETE
        [HttpDelete]
        public async Task Delete(int textbookId)
        {
            aztuAkademik.Textbook.FirstOrDefaultAsync(x => x.Id == textbookId).Result.DeleteDate = GetDate;
            aztuAkademik.Textbook.FirstOrDefaultAsync(x => x.Id == textbookId).Result.StatusId = 0;
            aztuAkademik.Textbook.FirstOrDefaultAsync(x => x.Id == textbookId).Result.RelTextbookResearcher.ToList().ForEach(x => x.DeleteDate = GetDate);

            await aztuAkademik.SaveChangesAsync();
        }


    }
}
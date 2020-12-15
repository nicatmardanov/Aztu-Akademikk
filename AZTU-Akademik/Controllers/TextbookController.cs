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

        private string IpAdress { get; }
        private string AInformation { get; }

        public TextbookController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
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
        public async Task Post([FromQuery] Textbook _textbook, [FromQuery] IQueryable<RelTextbookResearcher> _relTextbookResearchers)
        {
            _textbook.CreateDate = GetDate;
            _textbook.CreatorId = User_Id;
            await aztuAkademik.Textbook.AddAsync(_textbook);
            await aztuAkademik.SaveChangesAsync();


            await _relTextbookResearchers.ForEachAsync(x =>
            {
                x.CreateDate = GetDate;
                x.TextbookId = _textbook.Id;
            });


            await aztuAkademik.RelTextbookResearcher.AddRangeAsync(_relTextbookResearchers);
            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("Textbook", "", _textbook.Id, 1, User_Id, IpAdress, AInformation);
            await Classes.TLog.Log("RelTextbookResearcher", "", _relTextbookResearchers.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation);
        }




        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery] Textbook _textbook, [FromQuery] IQueryable<RelTextbookResearcher> _relTextbookResearchers, [FromQuery] long[] _deletedResearchers)
        {
            if (ModelState.IsValid)
            {

                aztuAkademik.Attach(_textbook);
                aztuAkademik.Entry(_textbook).State = EntityState.Modified;
                aztuAkademik.Entry(_textbook).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_textbook).Property(x => x.CreatorId).IsModified = false;
                aztuAkademik.Entry(_textbook).Property(x => x.PublisherId).IsModified = false;


                var entry = aztuAkademik.RelTextbookResearcher.Where(x => _deletedResearchers.Contains(x.Id));
                aztuAkademik.RelTextbookResearcher.RemoveRange(entry);
                await Classes.TLog.Log("RelTextbookResearcher", "", _deletedResearchers, 3, User_Id, IpAdress, AInformation);

                await _relTextbookResearchers.ForEachAsync(async x =>
                {
                    x.TextbookId = _textbook.Id;

                    if (x.Id == 0)
                    {
                        x.CreateDate = GetDate;
                        await Classes.TLog.Log("RelTextbookResearcher", "", x.Id, 1, User_Id, IpAdress, AInformation);
                    }

                    else
                    {
                        x.CreateDate = aztuAkademik.Project.FirstOrDefault(y => y.Id == x.Id).CreateDate;
                        x.UpdateDate = GetDate;
                        await Classes.TLog.Log("RelTextbookResearcher", "", x.Id, 2, User_Id, IpAdress, AInformation);
                    }

                });

                aztuAkademik.RelTextbookResearcher.UpdateRange(_relTextbookResearchers);
                await aztuAkademik.SaveChangesAsync();
                await Classes.TLog.Log("Textbook", "", _textbook.Id, 2, User_Id, IpAdress, AInformation);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int textbookId)
        {
            aztuAkademik.Textbook.FirstOrDefaultAsync(x => x.Id == textbookId).Result.DeleteDate = GetDate;
            aztuAkademik.Textbook.FirstOrDefaultAsync(x => x.Id == textbookId).Result.StatusId = 0;
            await aztuAkademik.Textbook.FirstOrDefaultAsync(x => x.Id == textbookId).Result.RelTextbookResearcher.AsQueryable().ForEachAsync(x => x.DeleteDate = GetDate);

            await aztuAkademik.SaveChangesAsync();
            await Classes.TLog.Log("Textbook", "", textbookId, 3, User_Id, IpAdress, AInformation);
        }


    }
}
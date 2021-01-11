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
        public class TextbookModel
        {
            public Textbook Textbook { get; set; }
            public List<RelTextbookResearcher> RelTextbookResearchers { get; set; }
            public long[] DeletedResearchers { get; set; }
        }
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
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Textbook(int user_id) => Json(aztuAkademik.RelTextbookResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Textbook).ThenInclude(x => x.Publisher).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x => x.Organization).AsNoTracking());



        //POST
        [HttpPost]
        public async Task Post(TextbookModel textbookModel)
        {
            textbookModel.Textbook.CreateDate = GetDate;
            textbookModel.Textbook.CreatorId = User_Id;
            await aztuAkademik.Textbook.AddAsync(textbookModel.Textbook).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);


            textbookModel.RelTextbookResearchers.ForEach(x =>
            {
                x.CreateDate = GetDate;
                x.TextbookId = textbookModel.Textbook.Id;
            });


            await aztuAkademik.RelTextbookResearcher.AddRangeAsync(textbookModel.RelTextbookResearchers).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Textbook", "", textbookModel.Textbook.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            await Classes.TLog.Log("RelTextbookResearcher", "", textbookModel.RelTextbookResearchers.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }




        //PUT
        [HttpPut]
        public async Task<int> Put(TextbookModel textbookModel)
        {
            if (ModelState.IsValid)
            {

                aztuAkademik.Attach(textbookModel.Textbook);
                aztuAkademik.Entry(textbookModel.Textbook).State = EntityState.Modified;
                aztuAkademik.Entry(textbookModel.Textbook).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(textbookModel.Textbook).Property(x => x.CreatorId).IsModified = false;
                aztuAkademik.Entry(textbookModel.Textbook).Property(x => x.PublisherId).IsModified = false;


                var entry = aztuAkademik.RelTextbookResearcher.Where(x => textbookModel.DeletedResearchers.Contains(x.Id));
                aztuAkademik.RelTextbookResearcher.RemoveRange(entry);
                await Classes.TLog.Log("RelTextbookResearcher", "", textbookModel.DeletedResearchers, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                textbookModel.RelTextbookResearchers.ForEach(async x =>
                {
                    x.TextbookId = textbookModel.Textbook.Id;

                    if (x.Id == 0)
                    {
                        x.CreateDate = GetDate;
                        await Classes.TLog.Log("RelTextbookResearcher", "", x.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                    }

                    else
                    {
                        x.CreateDate = aztuAkademik.Project.FirstOrDefault(y => y.Id == x.Id).CreateDate;
                        x.UpdateDate = GetDate;
                        await Classes.TLog.Log("RelTextbookResearcher", "", x.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                    }

                });

                aztuAkademik.RelTextbookResearcher.UpdateRange(textbookModel.RelTextbookResearchers);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Textbook", "", textbookModel.Textbook.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int textbookId)
        {
            Textbook textbook = await aztuAkademik.Textbook.Include(x => x.RelTextbookResearcher).
                FirstOrDefaultAsync(x => x.Id == textbookId && x.CreatorId == User_Id).
                ConfigureAwait(false);

            textbook.DeleteDate = GetDate;
            textbook.StatusId = 0;
            textbook.RelTextbookResearcher.ToList().ForEach(x => x.DeleteDate = GetDate);

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Textbook", "", textbookId, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }
}
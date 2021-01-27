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
            public Classes.Researchers Researchers { get; set; }
            public string Publisher { get; set; }
            public List<Urls> Urls { get; set; }
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
        public JsonResult Textbook(int user_id)
        {
            IQueryable<Textbook> textbooks = aztuAkademik.Textbook.
                Include(x => x.Publisher).
                Include(x => x.RelTextbookResearcher).
                Include(x => x.Creator).
                Include(x => x.Urls).
                Include(x => x.File).
                Where(x => x.CreatorId == user_id && !x.DeleteDate.HasValue).
                OrderByDescending(x => x.Id);


            return Json(textbooks.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.Date,
                Publisher = new
                {
                    x.Publisher.Id,
                    x.Publisher.Name
                },
                Researchers = new
                {
                    Internal = x.RelTextbookResearcher.Where(y => y.IntAuthorId > 0).Select(y => new
                    {
                        y.IntAuthor.Id,
                        y.IntAuthor.FirstName,
                        y.IntAuthor.LastName,
                        y.IntAuthor.Patronymic,
                        y.Type
                    }),

                    External = x.RelTextbookResearcher.Where(y => y.ExtAuthorId > 0).Select(y => new
                    {
                        y.ExtAuthor.Id,
                        y.ExtAuthor.Name,
                        y.Type
                    })
                },
                Urls = x.Urls.Select(y => new
                {
                    y.Url,
                    y.UrlType
                }),
                File = x.File.Name
            }));
        }



        //POST
        [HttpPost]
        public async Task Post([FromForm]TextbookModel textbookModel)
        {
            if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
            {
                File _file = new File
                {
                    Name = await Classes.FileSave.Save(Request.Form.Files[0], 7).ConfigureAwait(false),
                    Type = 8,
                    CreateDate = GetDate,
                    StatusId = 1,
                    UserId = User_Id
                };

                await aztuAkademik.File.AddAsync(_file).ConfigureAwait(false);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("File", "", _file.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                textbookModel.Textbook.FileId = _file.Id;
            }

            if (!string.IsNullOrEmpty(textbookModel.Publisher))
            {
                Publisher publisher = new Publisher
                {
                    Name = textbookModel.Publisher,
                    CreateDate=GetDate
                };

                await aztuAkademik.Publisher.AddAsync(publisher).ConfigureAwait(false);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Publisher", "", publisher.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                textbookModel.Textbook.PublisherId = publisher.Id;
            }


            textbookModel.Textbook.CreateDate = GetDate;
            textbookModel.Textbook.CreatorId = User_Id;
            await aztuAkademik.Textbook.AddAsync(textbookModel.Textbook).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Textbook", "", textbookModel.Textbook.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

            textbookModel.Urls.ForEach(x =>
            {
                x.TextbookId = textbookModel.Textbook.Id;
                x.CreateDate = GetDate;
            });

            await aztuAkademik.Urls.AddRangeAsync(textbookModel.Urls).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("URLS", "", textbookModel.Urls.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);


            RelTextbookResearcher relTextbookResearcher;

            if (textbookModel.Researchers.Internals != null)
                for (int i = 0; i < textbookModel.Researchers.Internals.Count; i++)
                {
                    relTextbookResearcher = new RelTextbookResearcher
                    {
                        TextbookId = textbookModel.Textbook.Id,
                        IntAuthorId = textbookModel.Researchers.Internals[i].Id,
                        CreateDate = GetDate,
                        StatusId = 1,
                        Type = textbookModel.Researchers.Internals[i].Type
                    };

                    await aztuAkademik.RelTextbookResearcher.AddAsync(relTextbookResearcher).ConfigureAwait(false);
                    await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                    await Classes.TLog.Log("RelTextbookResearcher", "", relTextbookResearcher.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                }

            if (textbookModel.Researchers.Externals != null)
                for (int i = 0; i < textbookModel.Researchers.Externals.Count; i++)
                {
                    relTextbookResearcher = new RelTextbookResearcher
                    {
                        TextbookId = textbookModel.Textbook.Id,
                        ExtAuthorId = textbookModel.Researchers.Externals[i].Id,
                        CreateDate = GetDate,
                        StatusId = 1,
                        Type = textbookModel.Researchers.Externals[i].Type
                    };

                    await aztuAkademik.RelTextbookResearcher.AddAsync(relTextbookResearcher).ConfigureAwait(false);
                    await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                    await Classes.TLog.Log("RelTextbookResearcher", "", relTextbookResearcher.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                }

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
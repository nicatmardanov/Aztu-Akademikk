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
    public class ArticleController : Controller
    {
        public class Internal
        {
            public int Id { get; set; }
            public bool Type { get; set; }
        }

        public class External
        {
            public int Id { get; set; }
            public bool Type { get; set; }
        }
        public class Researchers
        {
            public List<Internal> Internals { get; set; }
            public List<External> Externals { get; set; }
        }
        public class ArticleModel
        {
            public Article Article { get; set; }
            public List<ArticleUrl> Urls { get; set; }
            public long[] DeletedResearchers { get; set; }
            public bool FileChange { get; set; }
            public string Journal { get; set; }
            public bool Indexed { get; set; }
            public Researchers Researchers { get; set; }
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

        public ArticleController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }

        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Article([FromQuery] int UserId, [FromQuery]bool Indexed)
        {
            IQueryable<Article> article = aztuAkademik.Article.
                Where(x => x.CreatorId == UserId && x.JournalNavigation.Indexed == Indexed && !x.DeleteDate.HasValue).
                OrderByDescending(x => x.Id).AsNoTracking().
                Include(x => x.RelArticleResearcher).
                Include(x => x.File).
                Include(x => x.JournalNavigation).
                Include(x => x.ArticleUrl);



            //IQueryable<RelArticleResearcher> externalResearchers = article.RelArticleResearcher.AsQueryable().Where(x => x.ExtAuthorId > 0).
            //    Include(x => x.ExtAuthor);



            return Json(article.Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.Date,
                x.Volume,
                x.PageStart,
                x.PageEnd,
                Journal = new
                {
                    x.JournalNavigation.Id,
                    x.JournalNavigation.Name,
                },
                Urls = x.ArticleUrl.Select(y => new
                {
                    y.UrlType,
                    y.Url
                }),
                File = x.File.Name,
                Researchers = new
                {
                    Internal = x.RelArticleResearcher.Where(y => y.IntAuthorId > 0).
                    Select(y => new
                    {
                        y.IntAuthor.Id,
                        y.IntAuthor.FirstName,
                        y.IntAuthor.LastName,
                        y.IntAuthor.Patronymic,
                        y.Type
                    }),

                    External = x.RelArticleResearcher.Where(y => y.ExtAuthorId > 0).
                    Select(y => new
                    {
                        y.ExtAuthor.Id,
                        y.ExtAuthor.Name,
                        y.Type
                    })
                }
            }));
        }




        //POST
        [HttpPost]
        public async Task Post([FromForm]ArticleModel articleModel)
        {
            if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
            {
                File _file = new File
                {
                    Name = await Classes.FileSave.Save(Request.Form.Files[0], 5).ConfigureAwait(false),
                    Type = 6,
                    CreateDate = GetDate,
                    StatusId = 1,
                    UserId = User_Id
                };


                await aztuAkademik.File.AddAsync(_file).ConfigureAwait(false);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("File", "", _file.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                articleModel.Article.FileId = _file.Id;
            }

            if (!string.IsNullOrEmpty(articleModel.Journal))
            {
                Journal journal = new Journal
                {
                    Name = articleModel.Journal,
                    Indexed = articleModel.Indexed,
                    CreateDate = GetDate,
                    StatusId = 1
                };

                await aztuAkademik.Journal.AddAsync(journal).ConfigureAwait(false);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Journal", "", journal.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                articleModel.Article.Journal = journal.Id;
            }



            articleModel.Article.CreateDate = GetDate;
            articleModel.Article.CreatorId = User_Id;
            await aztuAkademik.Article.AddAsync(articleModel.Article).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Article", "", articleModel.Article.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

            articleModel.Urls.ForEach(x =>
            {
                x.ArticleId = articleModel.Article.Id;
                x.CreateDate = GetDate;
            });
            await aztuAkademik.ArticleUrl.AddRangeAsync(articleModel.Urls).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("ArticleURL", "", articleModel.Urls.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);


            RelArticleResearcher relArticleResearcher;

            if (articleModel.Researchers.Internals != null)
                for (int i = 0; i < articleModel.Researchers.Internals.Count; i++)
                {
                    relArticleResearcher = new RelArticleResearcher
                    {
                        ArticleId = articleModel.Article.Id,
                        IntAuthorId = articleModel.Researchers.Internals[i].Id,
                        CreateDate = GetDate,
                        StatusId = 1,
                        Type = articleModel.Researchers.Internals[i].Type
                    };

                    await aztuAkademik.RelArticleResearcher.AddAsync(relArticleResearcher).ConfigureAwait(false);
                    await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                    await Classes.TLog.Log("RelArticleResearcher", "", relArticleResearcher.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                }

            if (articleModel.Researchers.Externals != null)
                for (int i = 0; i < articleModel.Researchers.Externals.Count; i++)
                {
                    relArticleResearcher = new RelArticleResearcher
                    {
                        ArticleId = articleModel.Article.Id,
                        ExtAuthorId = articleModel.Researchers.Externals[i].Id,
                        CreateDate = GetDate,
                        StatusId = 1,
                        Type = articleModel.Researchers.Externals[i].Type
                    };

                    await aztuAkademik.RelArticleResearcher.AddAsync(relArticleResearcher).ConfigureAwait(false);
                    await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                    await Classes.TLog.Log("RelArticleResearcher", "", relArticleResearcher.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                }


        }

        //PUT
        [HttpPut]
        //public async Task<int> Put([FromForm]ArticleModel articleModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (articleModel.FileChange)
        //        {
        //            File _file = await aztuAkademik.File.FirstOrDefaultAsync(x => x.Id == articleModel.Article.FileId).ConfigureAwait(false);
        //            if (!string.IsNullOrEmpty(_file.Name))
        //                System.IO.File.Delete(_file.Name[1..]);

        //            _file.Name = await Classes.FileSave.Save(Request.Form.Files[0], 5).ConfigureAwait(false);
        //            _file.UpdateDate = GetDate;
        //            await Classes.TLog.Log("File", "", _file.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        //        }


        //        aztuAkademik.Attach(articleModel.Article);
        //        aztuAkademik.Entry(articleModel.Article).State = EntityState.Modified;
        //        aztuAkademik.Entry(articleModel.Article).Property(x => x.CreateDate).IsModified = false;
        //        aztuAkademik.Entry(articleModel.Article).Property(x => x.CreatorId).IsModified = false;
        //        aztuAkademik.Entry(articleModel.Article).Property(x => x.FileId).IsModified = false;



        //        IQueryable<RelArticleResearcher> entry = aztuAkademik.RelArticleResearcher.Where(x => articleModel.DeletedResearchers.Contains(x.Id));
        //        aztuAkademik.RelArticleResearcher.RemoveRange(entry);
        //        await Classes.TLog.Log("RelArticleResearcher", "", articleModel.DeletedResearchers, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);


        //        articleModel.RelArticleResearchers.ForEach(async x =>
        //        {
        //            x.ArticleId = articleModel.Article.Id;

        //            if (x.Id == 0)
        //            {
        //                x.CreateDate = GetDate;
        //                await Classes.TLog.Log("RelArticleResearcher", "", x.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        //            }

        //            else
        //            {
        //                x.CreateDate = aztuAkademik.Project.FirstOrDefault(y => y.Id == x.Id).CreateDate;
        //                x.UpdateDate = GetDate;
        //                await Classes.TLog.Log("RelArticleResearcher", "", x.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        //            }
        //        });

        //        aztuAkademik.RelArticleResearcher.UpdateRange(articleModel.RelArticleResearchers);
        //        await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);

        //        await Classes.TLog.Log("Article", "", articleModel.Article.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        //        return 1;
        //    }
        //    return 0;
        //}


        //DELETE
        [HttpDelete]
        public async Task Delete(int articleId)
        {
            Article article = await aztuAkademik.Article.Include(x => x.RelArticleResearcher).
                FirstOrDefaultAsync(x => x.Id == articleId && x.CreatorId == User_Id).ConfigureAwait(false);
            article.DeleteDate = GetDate;
            article.StatusId = 0;

            List<RelArticleResearcher> relArticleResearchers = article.RelArticleResearcher.ToList();
            relArticleResearchers.ForEach(x => x.DeleteDate = GetDate);

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Article", "", articleId, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            await Classes.TLog.Log("RelArticleResearcher", "", relArticleResearchers.Select(x => x.Id).ToArray(), 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }
}
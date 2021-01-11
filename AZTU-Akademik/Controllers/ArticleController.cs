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
        public class ArticleModel
        {
            public Article Article { get; set; }
            public IQueryable<RelArticleResearcher> RelArticleResearchers { get; set; }
            public long[] DeletedResearchers { get; set; } 
            public bool FileChange { get; set; }
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
        public JsonResult Article(int user_id) => Json(aztuAkademik.RelArticleResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Article).ThenInclude(x => x.Creator).
            Include(x => x.Article).ThenInclude(x => x.File).
            Include(x => x.Article).ThenInclude(x => x.Journal).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x => x.Organization).AsNoTracking());




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



            articleModel.Article.CreateDate = GetDate;
            articleModel.Article.CreatorId = User_Id;
            await aztuAkademik.Article.AddAsync(articleModel.Article).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);


            await articleModel.RelArticleResearchers.ForEachAsync(x =>
            {
                x.CreateDate = GetDate;
                x.ArticleId = articleModel.Article.Id;
            }).ConfigureAwait(false);


            await aztuAkademik.RelArticleResearcher.AddRangeAsync( articleModel.RelArticleResearchers).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);    

            await Classes.TLog.Log("Article", "", articleModel.Article.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            await Classes.TLog.Log("RelArticleResearcher", "",  articleModel.RelArticleResearchers.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

        }

        //PUT
        [HttpPut]
        public async Task<int> Put([FromForm]ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                if (articleModel.FileChange)
                {
                    File _file = await aztuAkademik.File.FirstOrDefaultAsync(x => x.Id == articleModel.Article.FileId).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(_file.Name))
                        System.IO.File.Delete(_file.Name[1..]);

                    _file.Name = await Classes.FileSave.Save(Request.Form.Files[0], 5).ConfigureAwait(false);
                    _file.UpdateDate = GetDate;
                    await Classes.TLog.Log("File", "", _file.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                }


                aztuAkademik.Attach(articleModel.Article);
                aztuAkademik.Entry(articleModel.Article).State = EntityState.Modified;
                aztuAkademik.Entry(articleModel.Article).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(articleModel.Article).Property(x => x.CreatorId).IsModified = false;
                aztuAkademik.Entry(articleModel.Article).Property(x => x.FileId).IsModified = false;



                IQueryable<RelArticleResearcher> entry = aztuAkademik.RelArticleResearcher.Where(x => articleModel.DeletedResearchers.Contains(x.Id));
                aztuAkademik.RelArticleResearcher.RemoveRange( articleModel.RelArticleResearchers);
                await Classes.TLog.Log("RelArticleResearcher", "", articleModel.DeletedResearchers, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);


                await  articleModel.RelArticleResearchers.ForEachAsync(async x =>
                {
                    x.ArticleId = articleModel.Article.Id;

                    if (x.Id == 0)
                    {
                        x.CreateDate = GetDate;
                        await Classes.TLog.Log("RelArticleResearcher", "", x.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                    }

                    else
                    {
                        x.CreateDate = aztuAkademik.Project.FirstOrDefault(y => y.Id == x.Id).CreateDate;
                        x.UpdateDate = GetDate;
                        await Classes.TLog.Log("RelArticleResearcher", "", x.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                    }
                }).ConfigureAwait(false);

                aztuAkademik.RelArticleResearcher.UpdateRange( articleModel.RelArticleResearchers);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);

                await Classes.TLog.Log("Article", "", articleModel.Article.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int articleId)
        {
            Article article = await aztuAkademik.Article.Include(x=>x.RelArticleResearcher).FirstOrDefaultAsync(x => x.Id == articleId && x.CreatorId==User_Id).ConfigureAwait(false);
            article.DeleteDate = GetDate;
            article.StatusId = 0;

            await article.RelArticleResearcher.AsQueryable().ForEachAsync(x => x.DeleteDate = GetDate).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Article", "", articleId, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }
}
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
        [HttpGet("Article")]
        [AllowAnonymous]
        public JsonResult Article(int user_id) => Json(aztuAkademik.RelArticleResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Article).ThenInclude(x => x.Creator).
            Include(x => x.Article).ThenInclude(x => x.File).
            Include(x => x.Article).ThenInclude(x => x.Journal).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x => x.Organization).AsNoTracking());




        //POST
        [HttpPost]
        public async Task Post([FromQuery] Article _article, [FromQuery] IQueryable<RelArticleResearcher> _relArticleResearchers)
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


                _article.FileId = _file.Id;
            }



            _article.CreateDate = GetDate;
            _article.CreatorId = User_Id;
            await aztuAkademik.Article.AddAsync(_article).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);


            await _relArticleResearchers.ForEachAsync(x =>
            {
                x.CreateDate = GetDate;
                x.ArticleId = _article.Id;
            }).ConfigureAwait(false);


            await aztuAkademik.RelArticleResearcher.AddRangeAsync(_relArticleResearchers).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);

            await Classes.TLog.Log("Article", "", _article.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            await Classes.TLog.Log("RelArticleResearcher", "", _relArticleResearchers.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

        }

        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery] Article _article, [FromQuery] IQueryable<RelArticleResearcher> _relArticleResearchers, [FromQuery] long[] _deletedResearchers, [FromQuery] bool fileChange)
        {
            if (ModelState.IsValid)
            {
                if (fileChange)
                {
                    File _file = await aztuAkademik.File.FirstOrDefaultAsync(x => x.Id == _article.FileId).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(_file.Name))
                        System.IO.File.Delete(_file.Name[1..]);

                    _file.Name = await Classes.FileSave.Save(Request.Form.Files[0], 5).ConfigureAwait(false);
                    _file.UpdateDate = GetDate;
                    await Classes.TLog.Log("File", "", _file.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                }


                aztuAkademik.Attach(_article);
                aztuAkademik.Entry(_article).State = EntityState.Modified;
                aztuAkademik.Entry(_article).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_article).Property(x => x.CreatorId).IsModified = false;
                aztuAkademik.Entry(_article).Property(x => x.FileId).IsModified = false;



                IQueryable<RelArticleResearcher> entry = aztuAkademik.RelArticleResearcher.Where(x => _deletedResearchers.Contains(x.Id));
                aztuAkademik.RelArticleResearcher.RemoveRange(_relArticleResearchers);
                await Classes.TLog.Log("RelArticleResearcher", "", _deletedResearchers, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);


                await _relArticleResearchers.ForEachAsync(async x =>
                {
                    x.ArticleId = _article.Id;

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

                aztuAkademik.RelArticleResearcher.UpdateRange(_relArticleResearchers);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);

                await Classes.TLog.Log("Article", "", _article.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int articleId)
        {
            Article article = await aztuAkademik.Article.Include(x=>x.RelArticleResearcher).FirstOrDefaultAsync(x => x.Id == articleId).ConfigureAwait(false);
            article.DeleteDate = GetDate;
            article.StatusId = 0;

            await article.RelArticleResearcher.AsQueryable().ForEachAsync(x => x.DeleteDate = GetDate).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Article", "", articleId, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }
}
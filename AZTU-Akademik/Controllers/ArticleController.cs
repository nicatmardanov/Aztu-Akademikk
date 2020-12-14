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


        //GET
        [HttpGet("Article")]
        [AllowAnonymous]
        public JsonResult Article(int user_id) => Json(aztuAkademik.RelArticleResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Article).ThenInclude(x => x.Creator).
            Include(x => x.Article).ThenInclude(x => x.File).
            Include(x => x.Article).ThenInclude(x => x.Journal).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x => x.Organization));




        //POST
        [HttpPost]
        public async Task Post([FromQuery] Article _article, [FromQuery] List<RelArticleResearcher> _relArticleResearchers)
        {

            if (Request.ContentLength > 0 && Request.Form.Files.Count > 0)
            {
                File _file = new File
                {
                    Name = await Classes.FileSave.Save(Request.Form.Files[0], 5),
                    Type = 6,
                    CreateDate = GetDate,
                    StatusId = 1,
                    UserId = User_Id
                };


                await aztuAkademik.File.AddAsync(_file);
                await aztuAkademik.SaveChangesAsync();


                _article.FileId = _file.Id;
            }



            _article.CreateDate = GetDate;
            _article.CreatorId = User_Id;
            await aztuAkademik.Article.AddAsync(_article);
            await aztuAkademik.SaveChangesAsync();


            _relArticleResearchers.ForEach(x =>
            {
                x.CreateDate = GetDate;
                x.ArticleId = _article.Id;
            });


            await aztuAkademik.RelArticleResearcher.AddRangeAsync(_relArticleResearchers);
            await aztuAkademik.SaveChangesAsync();
        }

        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery] Article _article, [FromQuery] List<RelArticleResearcher> _relArticleResearchers, [FromQuery] long[] _deletedResearchers, [FromQuery] bool fileChange)
        {
            if (ModelState.IsValid)
            {
                if (fileChange)
                {
                    File _file = await aztuAkademik.File.FirstOrDefaultAsync(x => x.Id == _article.FileId);
                    if (!string.IsNullOrEmpty(_file.Name))
                        System.IO.File.Delete(_file.Name[1..]);

                    _file.Name = await Classes.FileSave.Save(Request.Form.Files[0], 5);
                    _file.UpdateDate = GetDate;
                }


                aztuAkademik.Attach(_article);
                aztuAkademik.Entry(_article).State = EntityState.Modified;
                aztuAkademik.Entry(_article).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_article).Property(x => x.CreatorId).IsModified = false;
                aztuAkademik.Entry(_article).Property(x => x.FileId).IsModified = false;

                
                
                var entry = aztuAkademik.RelArticleResearcher.Where(x => _deletedResearchers.Contains(x.Id));
                aztuAkademik.RelArticleResearcher.RemoveRange(_relArticleResearchers);

                _relArticleResearchers.ForEach(x =>
                {
                    x.UpdateDate = GetDate;
                    x.ArticleId = _article.Id;

                    if (x.Id == 0)
                        x.CreateDate = GetDate;

                    else
                        x.CreateDate = aztuAkademik.Project.FirstOrDefault(y => y.Id == x.Id).CreateDate;
                });

                aztuAkademik.RelArticleResearcher.UpdateRange(_relArticleResearchers);
                await aztuAkademik.SaveChangesAsync();

                return 1;
            }
            return 0;
        }


        //DELETE
        [HttpDelete]
        public async Task Delete(int articleId)
        {
            aztuAkademik.Article.FirstOrDefaultAsync(x => x.Id == articleId).Result.DeleteDate = GetDate;
            aztuAkademik.Article.FirstOrDefaultAsync(x => x.Id == articleId).Result.StatusId = 0;
            aztuAkademik.Article.FirstOrDefaultAsync(x => x.Id == articleId).Result.RelArticleResearcher.ToList().ForEach(x => x.DeleteDate = GetDate);

            await aztuAkademik.SaveChangesAsync();
        }


    }
}
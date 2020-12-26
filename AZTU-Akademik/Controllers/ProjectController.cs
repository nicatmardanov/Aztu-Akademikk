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
    public class ProjectController : Controller
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

        public ProjectController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Project(int user_id) => Json(aztuAkademik.RelProjectResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Project).ThenInclude(x => x.Organization).
            Include(x => x.Project).ThenInclude(x => x.Researcher).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x => x.Organization).AsNoTracking());

        //[HttpGet("AllProjects")]
        //public JsonResult AllProjects() => Json(aztuAkademik.Project.Where(x => !x.DeleteDate.HasValue).
        //    Include(x => x.Organization).Include(x => x.Researcher));


        //POST
        [HttpPost]
        public async Task Post([FromQuery] Project _project, [FromQuery] IQueryable<RelProjectResearcher> _relProjectResearcher)
        {
            _project.CreateDate = GetDate;
            _project.ResearcherId = User_Id;
            await aztuAkademik.Project.AddAsync(_project).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);


            await _relProjectResearcher.ForEachAsync(x =>
            {
                x.CreateDate = GetDate;
                x.ProjectId = _project.Id;
            }).ConfigureAwait(false);


            await aztuAkademik.RelProjectResearcher.AddRangeAsync(_relProjectResearcher).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Project", "", _project.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
            await Classes.TLog.Log("RelProjectResearcher", "", _relProjectResearcher.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);


            //bool condition = intAuthors.Length > extAuthors.Length;
            //int length = condition ? intAuthors.Length : extAuthors.Length;

            //RelProjectResearcher relProjectResearcher;

            //for (int i = 0; i < length; i++)
            //{
            //    if (condition)
            //    {
            //        relProjectResearcher = new RelProjectResearcher
            //        {
            //            ProjectId = _project.Id,
            //            IntAuthorId = intAuthors[i],
            //            Type = leadAuthorsId.Contains(intAuthors[i]) ? (byte)1 : (byte)0,
            //            CreateDate = GetDate
            //        };

            //        await aztuAkademik.RelProjectResearcher.AddAsync(relProjectResearcher);
            //        await aztuAkademik.SaveChangesAsync();

            //        if (i < extAuthors.Length)
            //        {
            //            relProjectResearcher = new RelProjectResearcher
            //            {
            //                ProjectId = _project.Id,
            //                ExtAuthorId = extAuthors[i],
            //                Type = leadAuthorsId.Contains(intAuthors[i]) ? (byte)1 : (byte)0,
            //                CreateDate = GetDate
            //            };

            //            await aztuAkademik.RelProjectResearcher.AddAsync(relProjectResearcher);
            //            await aztuAkademik.SaveChangesAsync();
            //        }
            //    }
            //    else
            //    {
            //        relProjectResearcher = new RelProjectResearcher
            //        {
            //            ProjectId = _project.Id,
            //            ExtAuthorId = extAuthors[i],
            //            Type = leadAuthorsId.Contains(intAuthors[i]) ? (byte)1 : (byte)0,
            //            CreateDate = GetDate
            //        };

            //        await aztuAkademik.RelProjectResearcher.AddAsync(relProjectResearcher);
            //        await aztuAkademik.SaveChangesAsync();

            //        if (i < intAuthors.Length)
            //        {
            //            relProjectResearcher = new RelProjectResearcher
            //            {
            //                ProjectId = _project.Id,
            //                IntAuthorId = intAuthors[i],
            //                Type = leadAuthorsId.Contains(intAuthors[i]) ? (byte)1 : (byte)0,
            //                CreateDate = GetDate
            //            };

            //            await aztuAkademik.RelProjectResearcher.AddAsync(relProjectResearcher);
            //            await aztuAkademik.SaveChangesAsync();
            //        }
            //    }
            //}
        }


        //PUT
        [HttpPut]
        public async Task<int> Put([FromQuery] Project _project, [FromQuery] IQueryable<RelProjectResearcher> _relProjectResearchers, [FromQuery] long[] _deletedResearchers)
        {
            if (ModelState.IsValid)
            {

                aztuAkademik.Attach(_project);
                aztuAkademik.Entry(_project).State = EntityState.Modified;
                aztuAkademik.Entry(_project).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(_project).Property(x => x.ResearcherId).IsModified = false;


                var entry = aztuAkademik.RelProjectResearcher.Where(x => _deletedResearchers.Contains(x.Id));
                aztuAkademik.RelProjectResearcher.RemoveRange(entry);
                await Classes.TLog.Log("RelProjectResearcher", "", _deletedResearchers, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                await _relProjectResearchers.ForEachAsync(async x =>
                {
                    x.ProjectId = _project.Id;

                    if (x.Id == 0)
                    {
                        x.CreateDate = GetDate;
                        await Classes.TLog.Log("RelProjectResearcher", "", x.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                    }

                    else
                    {
                        x.CreateDate = aztuAkademik.Project.FirstOrDefault(y => y.Id == x.Id).CreateDate;
                        x.UpdateDate = GetDate;
                        await Classes.TLog.Log("RelProjectResearcher", "", x.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                    }

                }).ConfigureAwait(false);

                aztuAkademik.RelProjectResearcher.UpdateRange(_relProjectResearchers);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Project", "", _project.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }



        //DELETE
        [HttpDelete]
        public async Task Delete(int projectId)
        {
            Project project = await aztuAkademik.Project.Include(x => x.RelProjectResearcher).FirstOrDefaultAsync(x => x.Id == projectId).ConfigureAwait(false);
            project.DeleteDate = GetDate;
            project.StatusId = 0;
            await project.RelProjectResearcher.AsQueryable().ForEachAsync(x => x.DeleteDate = GetDate).ConfigureAwait(false);

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Project", "", projectId, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }
}
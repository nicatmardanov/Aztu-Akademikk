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


        //GET
        [HttpGet("Project")]
        [AllowAnonymous]
        public JsonResult Project(int user_id) => Json(aztuAkademik.RelProjectResearcher.Where(x => (x.IntAuthorId == user_id || x.ExtAuthorId == user_id) && !x.DeleteDate.HasValue).
            OrderByDescending(x => x.Id).
            Include(x => x.Project).ThenInclude(x => x.Organization).
            Include(x => x.Project).ThenInclude(x => x.Researcher).
            Include(x => x.IntAuthor).Include(x => x.ExtAuthor).ThenInclude(x=>x.Organization));

        //[HttpGet("AllProjects")]
        //public JsonResult AllProjects() => Json(aztuAkademik.Project.Where(x => !x.DeleteDate.HasValue).
        //    Include(x => x.Organization).Include(x => x.Researcher));


        //POST
        [HttpPost]
        public async Task Post([FromQuery] Project _project, [FromQuery] List<RelProjectResearcher> _relProjectResearcher)
        {
            _project.CreateDate = GetDate;
            _project.ResearcherId = User_Id;
            await aztuAkademik.Project.AddAsync(_project);
            await aztuAkademik.SaveChangesAsync();


            _relProjectResearcher.ForEach(x =>
            {
                x.CreateDate = GetDate;
                x.ProjectId = _project.Id;
            });


            await aztuAkademik.RelProjectResearcher.AddRangeAsync(_relProjectResearcher);
            await aztuAkademik.SaveChangesAsync();


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


        ////PUT
        //[HttpPut]
        //public async Task<int> Put([FromQuery] Project _project, [FromQuery] int[] intAuthors, [FromQuery] int[] extAuthors, int[] leadAuthors)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _project.UpdateDate = GetDate;
        //        aztuAkademik.Attach(_project);
        //        aztuAkademik.Entry(_project).State = EntityState.Modified;
        //        aztuAkademik.Entry(_project).Property(x => x.CreateDate).IsModified = false;
        //        aztuAkademik.Entry(_project).Property(x => x.ResearcherId).IsModified = false;




        //        await aztuAkademik.SaveChangesAsync();
        //    }

        //    return 0;
        //}



        //DELETE
        [HttpDelete]
        public async Task Delete(int projectId)
        {
            aztuAkademik.Project.FirstOrDefaultAsync(x => x.Id == projectId).Result.DeleteDate = GetDate;
            aztuAkademik.Project.FirstOrDefaultAsync(x => x.Id == projectId).Result.StatusId = 0;
            aztuAkademik.Project.FirstOrDefaultAsync(x => x.Id == projectId).Result.RelProjectResearcher.ToList().ForEach(x => x.DeleteDate = GetDate);

            await aztuAkademik.SaveChangesAsync();
        }


    }
}
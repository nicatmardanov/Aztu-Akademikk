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
        public class ProjectModel
        {
            public Project Project { get; set; }
            public Classes.Researchers Researchers { get; set; }
            public List<RelProjectResearcher> RelProjectResearchers { get; set; }
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

        public ProjectController(IHttpContextAccessor accessor)
        {
            IpAdress = !string.IsNullOrEmpty(accessor.HttpContext.Connection.RemoteIpAddress.ToString()) ? accessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
            AInformation = accessor.HttpContext.Request.Headers["User-Agent"].ToString();
        }


        //GET
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Project(int user_id) => Json(aztuAkademik.Project.
            Include(x => x.RelProjectResearcher).
            Include(x => x.Organization).
            Where(x => x.ResearcherId == user_id && !x.DeleteDate.HasValue).
            AsNoTracking().
            Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.StartDate,
                x.EndDate,
                Organization = new
                {
                    x.Organization.Id,
                    x.Organization.Name
                },
                Researchers = new
                {
                    Internals = x.RelProjectResearcher.Where(y => y.IntAuthorId > 0).Select(y => new
                    {
                        y.IntAuthor.Id,
                        y.IntAuthor.FirstName,
                        y.IntAuthor.LastName,
                        y.IntAuthor.Patronymic
                    }),
                    Externals = x.RelProjectResearcher.Where(y => y.ExtAuthorId > 0).Select(y => new
                    {
                        y.ExtAuthor.Id,
                        y.ExtAuthor.Name
                    })
                }
            }));

        //[HttpGet("AllProjects")]
        //public JsonResult AllProjects() => Json(aztuAkademik.Project.Where(x => !x.DeleteDate.HasValue).
        //    Include(x => x.Organization).Include(x => x.Researcher));


        //POST
        [HttpPost]
        public async Task Post(ProjectModel projectModel)
        {
            projectModel.Project.CreateDate = GetDate;
            projectModel.Project.ResearcherId = User_Id;

            await aztuAkademik.Project.AddAsync(projectModel.Project).ConfigureAwait(false);
            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Project", "", projectModel.Project.Id, 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);

            
            RelProjectResearcher relProjectResearcher;

            if (projectModel.Researchers.Internals != null)
                for (int i = 0; i < projectModel.Researchers.Internals.Count; i++)
                {
                    relProjectResearcher = new RelProjectResearcher
                    {
                        CreateDate = GetDate,
                        Type = projectModel.Researchers.Internals[i].Type,
                        ProjectId = projectModel.Project.Id,
                        IntAuthorId = projectModel.Researchers.Internals[i].Id,
                        StatusId = 1
                    };
                    await aztuAkademik.RelProjectResearcher.AddAsync(relProjectResearcher).ConfigureAwait(false);
                    await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                    await Classes.TLog.Log("RelProjectResearcher", "", projectModel.RelProjectResearchers.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                }

            if (projectModel.Researchers.Externals != null)
                for (int i = 0; i < projectModel.Researchers.Externals.Count; i++)
                {
                    relProjectResearcher = new RelProjectResearcher
                    {
                        CreateDate = GetDate,
                        Type = projectModel.Researchers.Externals[i].Type,
                        ProjectId = projectModel.Project.Id,
                        ExtAuthorId = projectModel.Researchers.Externals[i].Id,
                        StatusId = 1
                    };
                    await aztuAkademik.RelProjectResearcher.AddAsync(relProjectResearcher).ConfigureAwait(false);
                    await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                    await Classes.TLog.Log("RelProjectResearcher", "", projectModel.RelProjectResearchers.Select(x => x.Id).ToArray(), 1, User_Id, IpAdress, AInformation).ConfigureAwait(false);
                }


        }


        //PUT
        [HttpPut]
        public async Task<int> Put(ProjectModel projectModel)
        {
            if (ModelState.IsValid)
            {

                aztuAkademik.Attach(projectModel.Project);
                aztuAkademik.Entry(projectModel.Project).State = EntityState.Modified;
                aztuAkademik.Entry(projectModel.Project).Property(x => x.CreateDate).IsModified = false;
                aztuAkademik.Entry(projectModel.Project).Property(x => x.ResearcherId).IsModified = false;


                var entry = aztuAkademik.RelProjectResearcher.Where(x => projectModel.DeletedResearchers.Contains(x.Id));
                aztuAkademik.RelProjectResearcher.RemoveRange(entry);
                await Classes.TLog.Log("RelProjectResearcher", "", projectModel.DeletedResearchers, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                projectModel.RelProjectResearchers.ForEach(async x =>
                {
                    x.ProjectId = projectModel.Project.Id;

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

                });

                aztuAkademik.RelProjectResearcher.UpdateRange(projectModel.RelProjectResearchers);
                await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
                await Classes.TLog.Log("Project", "", projectModel.Project.Id, 2, User_Id, IpAdress, AInformation).ConfigureAwait(false);

                return 1;
            }
            return 0;
        }



        //DELETE
        [HttpDelete]
        public async Task Delete(int projectId)
        {
            Project project = await aztuAkademik.Project.Include(x => x.RelProjectResearcher).FirstOrDefaultAsync(x => x.Id == projectId && x.ResearcherId == User_Id).
                ConfigureAwait(false);
            project.DeleteDate = GetDate;
            project.StatusId = 0;
            project.RelProjectResearcher.ToList().ForEach(x => x.DeleteDate = GetDate);

            await aztuAkademik.SaveChangesAsync().ConfigureAwait(false);
            await Classes.TLog.Log("Project", "", projectId, 3, User_Id, IpAdress, AInformation).ConfigureAwait(false);
        }


    }
}
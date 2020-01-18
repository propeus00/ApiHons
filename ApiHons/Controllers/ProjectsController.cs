using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiHons.Models;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Newtonsoft.Json.Linq;


namespace ApiHons.Controllers
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public List<string> Skills { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public int TotalProjectsNumber { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly HonsProjContext _context;

        public ProjectsController(HonsProjContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projects>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/nameUser
        [HttpGet("{nameUser}")]
        public async Task<ActionResult<List<UserInfo>>> GetProjects(string nameUser)
        {

            var users = await (from user in _context.Users
                                where user.FullName.ToLower().Contains(nameUser)
                                join projects in _context.Projects on user.UserId equals projects.UserId into totalNumberProjects
                                select new UserInfo()
                                            {
                                                UserId = user.UserId,
                                              FullName =user.FullName,
                                               Title = user.Title,
                                               TotalProjectsNumber = totalNumberProjects.Count()
                                            }).ToListAsync();
            //Ex [3,2,6],[8,4,2,1],[4,5,2,1,3]
            var technologiesIdsLists = new List<IQueryable<int>>();
            var technologiesNamesLists = new List<List<string>>();
            var results = new List<UserInfo>();

            foreach (var user in users)
            {
               IQueryable<int> usersTechnologiesId =  from technology in _context.UsersTechnologies
                    where technology.UsersId == user.UserId
                    select technology.TechnologiesId;
               technologiesIdsLists.Add(usersTechnologiesId);
            }

            foreach (var techIdsList in technologiesIdsLists)
            {
                var listTechnologies = new List<string>();
                //[3,2,6]
                foreach (var id in techIdsList)
                {
                    
                    var techName = from techs in _context.Technologies
                        where techs.TechnologyId.Equals(id)
                        select techs.Name;
                    //id:3 name:React / id:2 name:Django etc
                    listTechnologies.Add(techName.First());
                }
                technologiesNamesLists.Add(listTechnologies);

            }

            int num = 0;
            foreach (var user in users)
            {
                user.Skills = technologiesNamesLists[num];
                num++;
                results.Add(user);
            }

           


            return results;
        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjects(int id, Projects projects)
        {
            if (id != projects.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(projects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        [HttpPost]
        public async Task<ActionResult<Projects>> PostProjects(Projects projects)
        {
            _context.Projects.Add(projects);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjects", new { id = projects.ProjectId }, projects);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Projects>> DeleteProjects(int id)
        {
            var projects = await _context.Projects.FindAsync(id);
            if (projects == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(projects);
            await _context.SaveChangesAsync();

            return projects;
        }

        private bool ProjectsExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}

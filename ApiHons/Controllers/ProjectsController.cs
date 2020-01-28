using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiHons.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiHons.Models;
using AutoMapper;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Newtonsoft.Json.Linq;


namespace ApiHons.Controllers
{
  
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly HonsProjContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(HonsProjContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projects>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
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

        //POST: api/Projects
       [HttpPost]
        public async Task<ActionResult<Projects>> PostProjects( [FromBody] ProjectCreateDto createProject)
        {

            // var finalProjectCreation = _mapper.Map<Entities.Projects>(createProject);
            var finalProjectCreation = new Projects();
            finalProjectCreation.Name = createProject.Name;
            finalProjectCreation.Description = createProject.Description;
            var userId = from user in _context.Users where user.FullName.ToLower() == createProject.UserName.ToLower() select user.UserId;
            finalProjectCreation.UserId = userId.FirstOrDefault();

            _context.Projects.Add(finalProjectCreation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjects", new { id = finalProjectCreation.ProjectId }, finalProjectCreation);
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

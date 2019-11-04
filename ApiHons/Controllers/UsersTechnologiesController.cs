using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiHons.Models;
using Microsoft.CodeAnalysis.CSharp;

namespace ApiHons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersTechnologiesController : ControllerBase
    {
        private readonly HonsProjContext _context;

        public UsersTechnologiesController(HonsProjContext context)
        {
            _context = context;
        }

        // GET: api/UsersTechnologies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersTechnologies>>> GetUsersTechnologies()
        {
            return await _context.UsersTechnologies.ToListAsync();
        }

        // GET: api/UsersTechnologies/userId
        [HttpGet("{userId}")]
        public List<string> GetUsersTechnologies(int userId)
        {
            //var userID = _context.Users.Where(x => x.FullName == userName).Select(x => x.UserId).FirstOrDefault();
            
            //use id here
            var techs = _context.UsersTechnologies.Where(x => x.UsersId == userId)
                .Select(x => x.TechnologiesId).ToList();

            var techNames = _context.Technologies.Where(x => techs.Contains(x.TechnologyId)).ToList();

            var list = new List<string>();
            foreach (var i in techNames)
            {
                list.Add(i.Name);
            }

            return list;
        }

        
    }
}

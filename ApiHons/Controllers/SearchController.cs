using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiHons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public class SearchController : ControllerBase
    {
        private readonly HonsProjContext _context;

        public SearchController(HonsProjContext context)
        {
            _context = context;
        }



        // GET: api/Search/nameUsers
        [HttpGet("{nameUser}")]
        public async Task<ActionResult<List<UserInfo>>> GetSearch(string nameUser)
        {
            var users = await(from user in _context.Users
                              where user.FullName.ToLower().Contains(nameUser)
                              join projects in _context.Projects on user.UserId equals projects.UserId into totalNumberProjects
                              select new UserInfo()
                              {
                                  UserId = user.UserId,
                                  FullName = user.FullName,
                                  Title = user.Title,
                                  TotalProjectsNumber = totalNumberProjects.Count()
                              }).ToListAsync();

            //Ex [3,2,6],[8,4,2,1],[4,5,2,1,3]
            var technologiesIdsLists = new List<IQueryable<int>>();
            //Ex [React, html, php], [DJango, Laravel etc]
            var technologiesNamesLists = new List<List<string>>();

            var results = new List<UserInfo>();

            foreach (var user in users)
            {
                IQueryable<int> usersTechnologiesId = from technology in _context.UsersTechnologies
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

        
    }
}

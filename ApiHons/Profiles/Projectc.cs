using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiHons.Controllers;
using AutoMapper;

namespace ApiHons.Profiles
{
    public class Project : Profile
    {
        public Project()
        {
            CreateMap<Models.ProjectCreateDto, Entities.Projects>();
        }
    }
}

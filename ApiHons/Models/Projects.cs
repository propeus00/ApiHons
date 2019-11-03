using System;
using System.Collections.Generic;

namespace ApiHons.Models
{
    public partial class Projects
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ApiHons.Models
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
    }
}

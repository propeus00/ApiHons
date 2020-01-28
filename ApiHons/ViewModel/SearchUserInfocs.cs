using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiHons.ViewModel
{
    public class SearchUserInfo
    {
        public int UserId { get; set; }
        public List<string> Skills { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public int TotalProjectsNumber { get; set; }
    }

}

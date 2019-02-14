using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALEmanMS.AdminWebsite.Models
{
    public class CategoryViewModel
    {
        public List<Category> Categories { get; set; }

        public List<GroupingGroup> GroupingGroups { get; set; }

        public List<GroupingGroup> LowGroupingGroups { get; set; }

        public List<Group> Groups { get; set; }

        public List<Unit> Units { get; set; }

    }
}
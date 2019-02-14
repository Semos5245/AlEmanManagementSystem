using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALEmanMS.AdminWebsite.Models
{
    public class StatisticViewModel
    {
        public List<StatisticItem> StatsticItemsCategories { get; set; }

        public List<StatisticItem> StatsticItemsCustomers { get; set; }

        public List<StatisticItem> StatsticItemsSenders { get; set; }

        public Journey Journey { get; set; }
    }
}
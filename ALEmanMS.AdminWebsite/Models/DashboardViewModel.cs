using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALEmanMS.AdminWebsite.Models
{
    public class DashboardViewModel
    {

        public int JouneysCount { get; set; }

        public int DriversCount { get; set; }

        public int SendersCount { get; set; }

        public int CustomersCount { get; set; }

        public int SenderCompanies { get; set; }

        public int ReceiverCompanies { get; set; }

        public int Categories { get; set; }

        public int Groups { get; set; }

        //public List<Currency> Currencies { get; set; }

        public List<JourneyStatistic> JourneyStatistics { get; set; }
    }
}
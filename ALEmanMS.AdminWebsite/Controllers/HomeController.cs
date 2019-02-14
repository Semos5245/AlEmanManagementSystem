using ALEmanMS.AdminWebsite.Models;
using ALEmanMS.AdminWebsite.Services;
using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public HomeController()
        {
            ViewBag.Page = "اللوحة الرئيسية";


        }

        #region HelperMethods
        // Function to return the statistics of journes during year 
        public List<JourneyStatistic> GetJourneyStatisticsInYear(int year)
        {
            // Get all journeys in that year 
            var journeys = db.Journeys.Where(j => j.JourneyDate.Year == year);

            List<JourneyStatistic> statistics = new List<JourneyStatistic>(); 

            for (int i = 0; i < 12; i++)
            {
                int month = i + 1;
                JourneyStatistic statistic = new JourneyStatistic
                {
                    Month = i + 1,
                    Value = journeys.Where(j => j.JourneyDate.Month == month).Count()
                };
                statistics.Add(statistic); 
            }

            return statistics; 
        }
        #endregion 

        public  ActionResult Index()
        {

            DashboardViewModel model = new DashboardViewModel
            {
                Categories = db.Categories.Count(),
                CustomersCount = db.People.OfType<Customer>().Count(),
                DriversCount = db.People.OfType<Driver>().Count(),
                Groups = db.Groups.Count(),
                JouneysCount = db.Journeys.Where(j => j.JourneyDate.Year == DateTime.Now.Year).Count(),
                ReceiverCompanies = db.ReceiverCompanies.Count(),
                SenderCompanies = db.SenderCompanies.Count(),
                SendersCount = db.People.OfType<Sender>().Count(),
                JourneyStatistics = GetJourneyStatisticsInYear(DateTime.Now.Year)
            };

            //CurrenyService service = new CurrenyService();
            //Currency DollarToSyp = await service.GetSYPCurrency();
            //Currency DollarToSar = await service.GetSARCurrency();
            //Currency DollarToLbp = await service.GetLBPCurrency();
            //Currency DollarToKwd = await service.GetKWDCurrency();
            //Currency DollarToEur = await service.GetEURCurrency();
            //Currency DollarToAud = await service.GetAUDCurrency();

            //decimal oneDollarInSar = 1 / DollarToSar.Value;
            //decimal oneDollarInLbp = 1 / DollarToLbp.Value;
            //decimal oneDollarInAud = 1 / DollarToAud.Value;
            //decimal oneDollarInKwd = 1 / DollarToKwd.Value;
            //decimal oneDollarInEur = 1 / DollarToEur.Value;

            //DollarToLbp.Value = DollarToSyp.Value / oneDollarInLbp;
            //DollarToSar.Value = DollarToSyp.Value / oneDollarInSar;
            //DollarToKwd.Value = DollarToSyp.Value / oneDollarInKwd;
            //DollarToEur.Value = DollarToSyp.Value / oneDollarInEur;
            //DollarToAud.Value = DollarToSyp.Value / oneDollarInAud;

            //model.Currencies = new List<Currency>
            //{
            //    DollarToSyp,
            //    DollarToAud,
            //    DollarToEur,
            //    DollarToKwd,
            //    DollarToLbp,
            //    DollarToSar,
            //}; 

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
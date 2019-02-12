using ALEmanMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALEmanMS.AdminWebsite.Controllers
{
    public class SettingsController : Controller
    {

        public SettingsController()
        {
            ViewBag.Page = "Settings"; 
        }

        // GET: Settings
        public ActionResult Index()
        {
            Setting setting = Setting.GetSettings(Server.MapPath("~/App_Data/Settings.json")); 
            return View(setting);
        }

        // POST: Settings/Edit 
        [HttpPost]
        public ActionResult Edit(decimal packagingPrice,decimal shippingPrice)
        {
            Setting setting = new Setting()
            {
                PackagingPrice = packagingPrice,
                ShippingPerKiloPrice = shippingPrice
            };

            setting.SaveSettings(Server.MapPath("~/App_Data/Settings.json"));

            return RedirectToAction("Index"); 

        }
        


    }
}
using ALEmanMS.AdminWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AKSoftware.WebApi.Client;
using System.Threading.Tasks;

namespace ALEmanMS.AdminWebsite.Services
{
    public class CurrenyService
    {

        string url = "https://www.amdoren.com/api/currency.php?api_key=kKSp5nRkTDSQKaHMKS6N4mLHwHYJip&from=USD&to="; 

        // Get SY from USD 
        public async Task<Currency> GetSYPCurrency()
        {
            ServiceClient client = new ServiceClient(url);

            var amount = await client.Get<ApiResponse>("SYP");
            //var text = await client.GetJsonResult("SYP"); 
            return new Currency
            {
                Name = "الليرة السورية",
                Flag = "Content/Assets/images/flags/syria.png",
                Value = amount.Amount
            }; 
        }

        // Get AUD from USD 
        public async Task<Currency> GetAUDCurrency()
        {
            ServiceClient client = new ServiceClient(url);

            var amount = await client.Get<ApiResponse>("AED");

            return new Currency
            {
                Name = "الدرهم الإماراتي",
                Flag = "Content/Assets/images/flags/uae.png",
                Value = amount.Amount
            };
        }

        // Get EUR from USD 
        public async Task<Currency> GetEURCurrency()
        {
            ServiceClient client = new ServiceClient(url);

            var amount = await client.Get<ApiResponse>("EUR");

            return new Currency
            {
                Name = "اليورو",
                Flag = "Content/Assets/images/flags/eu.png",
                Value = amount.Amount
            };
        }

        // Get KWD from USD 
        public async Task<Currency> GetKWDCurrency()
        {
            ServiceClient client = new ServiceClient(url);

            var amount = await client.Get<ApiResponse>("KWD");

            return new Currency
            {
                Name = "الدينار الكويتي",
                Flag = "Content/Assets/images/flags/kuwait.png",
                Value = amount.Amount
            };
        }

        // Get LBP from USD 
        public async Task<Currency> GetLBPCurrency()
        {
            ServiceClient client = new ServiceClient(url);

            var amount = await client.Get<ApiResponse>("LBP");

            return new Currency
            {
                Name = "الليرة اللبنانية",
                Flag = "Content/Assets/images/flags/lebanon.png",
                Value = amount.Amount
            };
        }

        // Get SAR from USD 
        public async Task<Currency> GetSARCurrency()
        {
            ServiceClient client = new ServiceClient(url);

            var amount = await client.Get<ApiResponse>("SAR");

            return new Currency
            {
                Name = "الريال السعودي",
                Flag = "Content/Assets/images/flags/saudi.png",
                Value = amount.Amount
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; 

namespace ALEmanMS.Models
{
    public class Setting
    {

        public decimal PackagingPrice { get; set; }

        public decimal ShippingPerKiloPrice { get; set; }
        
        // Save the settings 
        public void SaveSettings(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                string jsonData = JsonConvert.SerializeObject(this);
                writer.Write(jsonData); 
            }
        }

        // Get the settings 
        public static Setting GetSettings(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string jsonData = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<Setting>(jsonData); 
            }
        }
    }
}

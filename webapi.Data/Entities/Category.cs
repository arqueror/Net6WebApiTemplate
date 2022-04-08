using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace webapi.Data.Entities
{
    [Serializable]
    public class Category 
    {

        [JsonProperty("strCategory")]
        public string CategoryName { get; set; }
    }
}

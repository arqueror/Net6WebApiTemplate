using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace webapi.Data.Entities
{
    [Serializable]
    public class Glass 
    {

        [JsonProperty("strGlass")]
        public string GlassName { get; set; }

    }
}

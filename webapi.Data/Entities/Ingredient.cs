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
    public class Ingredient 
    {

        [JsonProperty("idIngredient")]
        public string IdIngredient { get; set; }

        [JsonProperty("strIngredient")]
        public string IngredientName { get; set; }

        [JsonProperty("strIngredient1")]
        private string IngredientName2 { set { IngredientName = value; } }

        [JsonProperty("strDescription")]
        private string Description { get; set; }

        [JsonProperty("strAlcohol")] public string Alcohol { get; set; } = "Alcoholic" ?? "Non Alcoholic";

        [JsonProperty("strABV")]
        private string ABV { get; set; }

    }
}

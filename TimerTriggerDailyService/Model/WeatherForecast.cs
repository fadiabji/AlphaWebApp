using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TimerNewsApp.Model
{
    public class WeatherForecast
    {
        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("temperatureC")]
        public int TemperatureC { get; set; }

        [JsonPropertyName("temperatureF")]
        public int TemperatureF { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("windSpeed")]

        public int WindSpeed { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }



        [JsonPropertyName("lang")]
        public string Lang { get; set; }
    }
}

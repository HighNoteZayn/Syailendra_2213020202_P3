using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Personal.Services
{
    public class CuacaInfo
    {
        public string Kota { get; set; }
        public string Kondisi { get; set; }
        public double Suhu { get; set; } // dalam °C
        public int Kelembapan { get; set; } // dalam %
    }

    public class CuacaService
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _apiKey = "ISI_API_KAMU"; // Ganti dengan API key OpenWeatherMap

        public async Task<CuacaInfo> GetCuacaAsync(string kota)
        {
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q= {kota}&appid={_apiKey}&units=metric&lang=id";
                var response = await _httpClient.GetStringAsync(url);
                var json = JObject.Parse(response);

                return new CuacaInfo
                {
                    Kota = json["name"].ToString(),
                    Kondisi = json["weather"][0]["description"].ToString(),
                    Suhu = double.Parse(json["main"]["temp"].ToString()),
                    Kelembapan = int.Parse(json["main"]["humidity"].ToString())
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Gagal mengambil data cuaca.", ex);
            }
        }
    }
}
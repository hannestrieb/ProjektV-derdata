using ProjektVäderdataDataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VäderLabb
{
    public class WeatherAnalysis
    {
        private DatabaseContext _context;
        public const string Outdoors = "Ute";
        public const string Indoors = "Inne";
        public WeatherAnalysis(DatabaseContext context) //Konstruktor
        {
            _context = context;
        }
        public void DisplayAverageTemp(string place)
        {
            Console.Clear();
            Console.WriteLine($"Genomsnittstemperatur {place.ToLower()}.\n\n");
            Console.Write("Ange ett 8-siffrigt datum (yyyyMMdd): ");

            string input = Console.ReadLine();

            //Kontrollerar att det är exakt 8 siffror
            if (input.Length != 8 || !input.All(char.IsDigit))
            {
                Console.WriteLine("Felaktigt format! Skriv 8 siffror.");
                return;
            }

            //Formaterar om inputen till yyyy-MM-dd, så användaren slipper skriva "-"
            string formattedDate = $"{input.Substring(0, 4)}-{input.Substring(4, 2)}-{input.Substring(6, 2)}";

            if (!DateTime.TryParseExact(formattedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
            {
                Console.WriteLine("Felaktigt datumformat! Exempel: 2016-11-29");
                return;
            }

            //Hitta alla poster samma dag oavsett tid
            var data = _context.WeatherRecords
                .Where(w => w.Plats == place &&
                    w.Datum.Date == searchDate.Date &&
                    w.Temp.HasValue).ToList();

            if (data.Any())
            {
                Console.Clear();

                //Csv filen innehåller dubbletter, så tar bort dessa men behåller unika Datum och Plats
                var uniqueData = data
                    .GroupBy(w => new { w.Datum, w.Plats })
                    .Select(g => g.First())
                    .ToList();

                double avgTemp = uniqueData.Average(w => w.Temp.Value);

                Console.WriteLine($"Medeltemperatur {place.ToLower()} för {searchDate:yyyy-MM-dd} är {avgTemp:F1}°C");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Ingen data hittades för detta datum.");
            }
        }

        public void DisplaySortedTemp(string place)
        {
            Console.Clear();
            Console.WriteLine($"Sortering av varmaste till kallaste dag enligt medeltemperatur {place.ToLower()}.\n\n");

            //Hämtar alla mätningar från vald plats med giltig temperatur
            var allData = _context.WeatherRecords
                .Where(w => w.Plats == place && w.Temp.HasValue)
                .ToList();

            //Tar bort dubbletter
            var uniqueData = allData
                .GroupBy(w => new { w.Datum, w.Plats })
                .Select(g => g.First())
                .ToList();

            var tempData = uniqueData
                .GroupBy(w => w.Datum.Date)  //Grupperar per dag
                .Select(g => new
                {
                    Date = g.Key,
                    AvgTemp = g.Average(w => w.Temp.Value)  //Beräknar medeltemperatur per dag
                })
                .OrderByDescending(x => x.AvgTemp)  //Sorterar efter varmast först
                .Take(10)  // Ta top 10
                .ToList();

            if (!tempData.Any())
            {
                Console.WriteLine("Ingen data hittades.");
                return;
            }

            //Visar resultatet
            int i = 1;
            foreach (var item in tempData)
            {
                Console.WriteLine($"{i,3}. {item.Date:yyyy-MM-dd}  {item.AvgTemp:F1}°C");
                i++;
            }
            Console.WriteLine($"\nTotalt antal dagar: {tempData.Count}");
        }

        public void DisplayAverageHumidity(string place)
        {
            Console.Clear();
            Console.WriteLine($"Sortering av torraste till fuktigaste dag enligt medelfuktighet {place.ToLower()}.\n\n");

            var allData = _context.WeatherRecords
                .Where(w => w.Plats == place && w.Luftfuktighet.HasValue)
                .ToList();

            //Tar bort dubbletter
            var uniqueData = allData
                .GroupBy(w => new { w.Datum, w.Plats })
                .Select(g => g.First())
                .ToList();

            var humidityData = uniqueData
                .GroupBy(w => w.Datum.Date)  //Gruppera per dag
                .Select(g => new
                {
                    Date = g.Key,
                    AvgHumidity = g.Average(w => w.Luftfuktighet.Value)  //Beräknar medelluftfuktighet per dag
                })
                .OrderBy(x => x.AvgHumidity)  //Sorterar torraste först, dvs lägst fuktighet
                .Take(10)  //Top 10
                .ToList();

            if (!humidityData.Any())
            {
                Console.WriteLine("Ingen data hittades.");
                return;
            }

            //Visar resultatet
            int i = 1;
            foreach (var item in humidityData)
            {
                Console.WriteLine($"{i,3}. {item.Date:yyyy-MM-dd}  {item.AvgHumidity:F1}%");
                i++;
            }
            Console.WriteLine($"\nTotalt antal dagar: {humidityData.Count}");
        }
        public void DisplaySortedMoldRisk(string place)
        {
            Console.Clear();
            Console.WriteLine($"Sortering av dagar med minst till högst mögelrisk {place.ToLower()}\n\n");

            var moldData = _context.WeatherRecords
                .Where(w => w.Plats == place && w.Temp.HasValue && w.Luftfuktighet.HasValue)
                .ToList();

            if (!moldData.Any())
            {
                Console.WriteLine("Ingen data hittades.");
                return;
            }

            //Tar bort dubbletter
            var uniqueData = moldData
                .GroupBy(w => new { w.Datum, w.Plats })
                .Select(g => g.First())
                .ToList();

            //Grupperar per dag och beräknar genomsnittlig mögelrisk
            var riskSorted = uniqueData
                .GroupBy(w => w.Datum.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    AvgRisk = g.Average(w => w.MoldRisk),
                    AvgTemp = g.Average(w => w.Temp.Value),
                    AvgHum = g.Average(w => w.Luftfuktighet.Value)
                })
                .OrderBy(x => x.AvgRisk)  //Minst risk först
                .ToList();

            //Visar resultatet
            int i = 1;
            foreach (var item in riskSorted)
            {
                Console.WriteLine($"{i,3}. {item.Date:yyyy-MM-dd}  Risk: {item.AvgRisk:F2}  (Temp: {item.AvgTemp:F1}°C, Fukt: {item.AvgHum:F1}%)");
                i++;
            }

            Console.WriteLine($"\nTotalt antal dagar: {riskSorted.Count}");

        }

        public void DisplayMeteorologicalSeasons()
        {
            Console.Clear();
            Console.WriteLine("Meteorologiska årstider ute");
            Console.WriteLine();

            var outdoorData = _context.WeatherRecords
                .Where(w => w.Plats == "Ute" && w.Temp.HasValue) //Utomhus
                .ToList();

            if (!outdoorData.Any())
            {
                Console.WriteLine("Ingen data hittades.");
                return;
            }

            //Tar bort dubbletter
            var uniqueData = outdoorData
                .GroupBy(w => new { w.Datum, w.Plats })
                .Select(g => g.First())
                .ToList();

            //Beräknar medeltemperatur per dag
            var dailyAvgTemp = uniqueData
                .GroupBy(w => w.Datum.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    AvgTemp = Math.Round(g.Average(w => w.Temp.Value), 1) //Avrundar till en decimal enl smhi
                })
                .OrderBy(x => x.Date)
                .ToList();

            DateTime? autumnStart = null;
            int fiveAutumnDays = 0; //Antal följande höstdagar som uppfyller kriteriet 5

            DateTime? winterStart = null;
            int fiveWinterDays = 0; //Antal följande vinterdagar som uppfyller kriteriet 5

            for (int i = 0; i < dailyAvgTemp.Count; i++)
            {
                var day = dailyAvgTemp[i];

                //Höst
                if (day.AvgTemp <= 10.0)
                {
                    if (fiveAutumnDays == 0)
                    {
                        autumnStart = day.Date;
                    }

                    fiveAutumnDays++;
                }
                else
                {
                    fiveAutumnDays = 0;
                    autumnStart = null;
                }

                //Vinter
                if (day.AvgTemp <= 0.0)
                {
                    if (fiveWinterDays == 0)
                    {
                        winterStart = day.Date;
                    }

                    fiveWinterDays++;
                }
                else
                {
                    fiveWinterDays = 0;
                    winterStart = null;
                }
            }

            //Resultat
            if (autumnStart.HasValue)
            {
                Console.WriteLine($"Startdatum för meteorologisk höst: {autumnStart.Value:yyyy-MM-dd}");
            }
            else
            {
                Console.WriteLine("Startdatum för meteorologisk höst inträffade inte under mätperioden.");
            }

            if (winterStart.HasValue)
            {
                Console.WriteLine($"Startdatum för meteorologisk vinter: {winterStart.Value:yyyy-MM-dd}");
            }
            else
            {
                Console.WriteLine("Startdatum för meteorologisk vinter inträffade inte under mätperioden.");
            }
        }
    }
}

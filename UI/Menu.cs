using System;
using System.Collections.Generic;

namespace VäderLabb
{
    class Menu
    {
        private WeatherAnalysis _analysis;
        public Menu(WeatherAnalysis analysis)
        {
            _analysis = analysis;
        }
        public int ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════╗");
            Console.WriteLine("║           Väderdata            ║");
            Console.WriteLine("╚════════════════════════════════╝\n");

            Console.WriteLine("1. Visa genomsnittstemperaturen genom att söka efter ett datum");
            Console.WriteLine("2. Visa dagar sorterade efter varmast till kallast genomsnittstemperatur");
            Console.WriteLine("3. Visa dagar sorterade efter torrast till fuktigast genomsnittsluftfuktighet");
            Console.WriteLine("4. Visa dagar sorterade efter minst till störst mögelrisk");
            Console.WriteLine("5. Visa startdatum för meterologiska årstider");
            Console.WriteLine("6. Exit");

            Console.Write("\nVälj ett alternativ genom att trycka på en siffra: ");

            ConsoleKeyInfo key = Console.ReadKey(true);

            //Försök konvertera den tryckta tangenten till ett nummer
            if (int.TryParse(key.KeyChar.ToString(), out int number))
            {
                return number;
            }

            return -1; //Ogiltigt val
        }

        public void DisplayAverageTempMenu() //Gemensam metod för att välja inomhus/utomhus genomsnittstemperatur
        {
            Console.Clear();
            Console.WriteLine("Vill du se genomsnittstemperatur ute eller inne?\n");
            Console.WriteLine("1. Ute");
            Console.WriteLine("2. Inne");

            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D1)
            {
                _analysis.DisplayAverageTemp(WeatherAnalysis.Outdoors);
            }
            else if (key == ConsoleKey.D2)
            {
                _analysis.DisplayAverageTemp(WeatherAnalysis.Indoors);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Felaktig input! Vänligen välj mellan 1 och 2.");
            }
        }
        public void DisplaySortedTempMenu() //Gemensam metod för att välja inomhus/utomhus sorterad temperatur
        {
            Console.Clear();
            Console.WriteLine("Vill du se sortering av varmaste till kallaste dagen ute eller inne?\n");
            Console.WriteLine("1. Ute");
            Console.WriteLine("2. Inne");
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D1)
            {
                _analysis.DisplaySortedTemp(WeatherAnalysis.Outdoors);
            }
            else if (key == ConsoleKey.D2)
            {
                _analysis.DisplaySortedTemp(WeatherAnalysis.Indoors);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Felaktig input! Vänligen välj mellan 1 och 2.");
            }
        }

        public void DisplayAverageHumidityMenu() //Medelluftfuktighet
        {
            Console.Clear();
            Console.WriteLine("Vill du se medelluftfuktighet ute eller inne?\n");
            Console.WriteLine("1. Ute");
            Console.WriteLine("2. Inne");
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D1)
            {
                _analysis.DisplayAverageHumidity(WeatherAnalysis.Outdoors);
            }
            else if (key == ConsoleKey.D2)
            {
                _analysis.DisplayAverageHumidity(WeatherAnalysis.Indoors);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Felaktig input! Vänligen välj mellan 1 och 2.");
            }
        }

        public void DisplayMoldRiskMenu() //Mögelrisk
        {
            Console.Clear();
            Console.WriteLine("Vill du se mögelrisk ute eller inne?\n");
            Console.WriteLine("1. Ute");
            Console.WriteLine("2. Inne");
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D1)
            {
                _analysis.DisplaySortedMoldRisk(WeatherAnalysis.Outdoors);
            }
            else if (key == ConsoleKey.D2)
            {
                _analysis.DisplaySortedMoldRisk(WeatherAnalysis.Indoors);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Felaktig input! Vänligen välj mellan 1 och 2.");
            }
        }
    }
}



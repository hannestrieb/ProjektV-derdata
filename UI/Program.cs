using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using ProjektVäderdataDataAccess;

namespace VäderLabb
{
    public class Program
    {
        private static DatabaseContext _context;
        private static WeatherAnalysis _analysis;
        private static DatabaseInitializer _initializer;
        static void Main(string[] args)
        {
            var filePath = "../../../TempFuktData.csv";

            try
            {
                _initializer = new DatabaseInitializer();
                _context = _initializer.InitializeDatabase();
                _initializer.InitializeData(filePath);
                _analysis = new WeatherAnalysis(_context);

                Console.WriteLine("Tryck valfri tangent för att fortsätta till menyn...");
                Console.ReadKey(true);
                RunApplication();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod vid läsning av filen: {ex.ToString()}");
                Console.WriteLine("\n\nTryck på valfri tangent för att avsluta...");
                Console.ReadKey();
                return;
            }
        }

        private static void RunApplication()
        {
            Menu menu = new Menu(_analysis);
            bool running = true;

            while (running)
            {
                int choice = menu.ShowMenu();

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        menu.DisplayAverageTempMenu();
                        Console.WriteLine("\n\nTryck på valfri knapp för att återgå till menyn...");
                        Console.ReadKey(true);
                        break;

                    case 2:
                        Console.Clear();
                        menu.DisplaySortedTempMenu();
                        Console.WriteLine("\n\nTryck på valfri knapp för att återgå till menyn...");
                        Console.ReadKey(true);
                        break;

                    case 3:
                        Console.Clear();
                        menu.DisplayAverageHumidityMenu();
                        Console.WriteLine("\n\nTryck på valfri knapp för att återgå till menyn...");
                        Console.ReadKey(true);
                        break;

                    case 4:
                        Console.Clear();
                        menu.DisplayMoldRiskMenu();
                        Console.WriteLine("\n\nTryck på valfri knapp för att återgå till menyn...");
                        Console.ReadKey(true);
                        break;

                    case 5:
                        Console.Clear();
                        _analysis.DisplayMeteorologicalSeasons();
                        Console.WriteLine("\n\nTryck på valfri knapp för att återgå till menyn...");
                        Console.ReadKey(true);
                        break;

                    case 6:
                        Console.Clear();
                        Console.WriteLine("Avslutar programmet...");
                        running = false;
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Ogiltigt val.");
                        Console.WriteLine("\n\nTryck på valfri knapp för att återgå till menyn...");
                        Console.ReadKey(true);
                        break;
                }
            }
        }
    }
}


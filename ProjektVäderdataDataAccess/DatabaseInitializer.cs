using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VäderLabb;

namespace ProjektVäderdataDataAccess
{
    public class DatabaseInitializer
    {
        private readonly DatabaseContext _context;

        public DatabaseInitializer() 
        {  
            _context = new DatabaseContext();
        }
        public DatabaseContext InitializeDatabase()
        {
            Console.WriteLine("Kontrollerar databas...");
            _context.Database.EnsureCreated();
            Console.WriteLine("Databas är klar.");
            return _context;
        }

        public void InitializeData(string filePath)
        {
            if (_context.WeatherRecords.Any())
            {
                Console.WriteLine($"Databasen innehåller redan {_context.WeatherRecords.Count()} mätningar\n");
                return;  //Avslutar utan att importera data igen för att undvika dubletter.
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Filen kunde inte hittas.", filePath);
            }

            Console.WriteLine("Importerar data från CSV...");

            var fileText = File.ReadAllText(filePath); //Omvandlar unicode minustecken till vanligt minustecken.
            //Fick hjälp av Noah och Philip här eftersom vi jämförde antal minustecken i SSMS och jag hade färre än dem.
            fileText = fileText
                .Replace('\u2212', '-')
                .Replace('\u2013', '-')
                .Replace('\u2014', '-')
                .Replace('\u2010', '-');

            using (var reader = new StringReader(fileText)) //StringReader för att läsa från strängen istället för filen direkt.
            {
                var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ","
                }
                    );

                csv.Context.RegisterClassMap<WeatherRecordMap>();
                var records = csv.GetRecords<WeatherRecord>().ToList();
                _context.WeatherRecords.AddRange(records);
                _context.SaveChanges();


                Console.WriteLine($"Importerade {records.Count} rader!\n");
            }
        }
    }
}

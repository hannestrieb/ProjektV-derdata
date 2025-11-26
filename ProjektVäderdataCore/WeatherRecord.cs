using System;
using System.Collections.Generic;
using System.Text;
using ProjektVäderdataCore;

namespace VäderLabb
{
    public class WeatherRecord
    {
        //Primary key, index, osv tilldelas i DatabaseContext.OnModelCreating()
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Plats { get; set; }
        public double? Temp { get; set; } //Nullable för att hantera saknade värden
        public double? Luftfuktighet { get; set; } //Nullable för att hantera saknade värden
        public double MoldRisk
        {
            get
            {
                //Beräknar mögelrisk
                if (Temp.HasValue && Luftfuktighet.HasValue)
                {
                    return MoldIndex.CalculateMoldRisk((double)Temp, (double)Luftfuktighet);
                }
                return 0.0;
            }
        }
    }
}

using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HqPlusAssignment_Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            HotelParser HotelParser = new HotelParser();
            
            HotelParser.fetchHtml("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html");
            
            if (HotelParser.requestSucceeded) 
            {
                HotelParser.ExtractHotelData();

                string output = JsonConvert.SerializeObject(HotelParser.Hotel, Newtonsoft.Json.Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });

                System.IO.File.WriteAllText(@"Output.json", output);
            }
            else
            {
                Console.WriteLine("failed to fetch html file");   
            }
        }
    }
}
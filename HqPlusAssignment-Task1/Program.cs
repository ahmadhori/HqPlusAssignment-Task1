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
            HotelParser hotelParser = new HotelParser();

            hotelParser.fetchHtml("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html");
            
            if (hotelParser.RequestSucceeded) 
            {
                hotelParser.ExtractHotelData();

                string output = JsonConvert.SerializeObject(hotelParser.Hotel, Newtonsoft.Json.Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });
                System.IO.File.WriteAllText(@"Output.json", output);


                if (hotelParser.IsValid())
                {
                    Console.WriteLine("Extracted Data Successfully to Output.json");
                }
                else
                {
                    Console.WriteLine("Failed to Extract Data with the following errors:");
                    foreach (var item in hotelParser.Errors)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
            else
            {
                Console.WriteLine("failed to fetch html file");   
            }
        }
    }
}
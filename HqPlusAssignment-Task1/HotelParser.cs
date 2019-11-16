
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HqPlusAssignment_Task1
{
    public class HotelParser
    {
        public bool requestSucceeded = false;
        private readonly HtmlDocument HtmlDoc;
        public readonly Hotel Hotel = new Hotel();
        public ICollection<string> Errors = new List<string>();

        public HotelParser()
        {
            HtmlDoc = new HtmlDocument();
        }

        public void fetchHtml(string Url)
        {
            string text;
            try
            {
                if (Url.StartsWith("https://") || Url.StartsWith("http://"))
                {
                    using (WebClient client = new WebClient())
                    {
                        text = client.DownloadString(Url);
                    }
                    Hotel.BookingPageUrl = Url;
                }
                else
                {
                    text = File.ReadAllText(Url);
                }
                HtmlDoc.LoadHtml(text);
                requestSucceeded = true;
            }
            catch (IOException e)
            {
                requestSucceeded = false;
            }
        }


        public bool ExtractHotelData()
        {
            this.ExtractName().ExtractAddress().ExtractClassification()
                    .ExtractReviewPoints().ExtractNumberOfReviews().ExtractDescription()
                    .ExtractRoomCategories().ExtractAlternativeHotels();

            return IsValid();
        }




        public HotelParser ExtractName()
        {
            var HotelNameDiv = HtmlDoc.GetElementbyId("hp_hotel_name");
            if (HotelNameDiv == null || HotelNameDiv.InnerText.Trim() == "")
            {
                Errors.Add("couldn't find hotel name");
                return this;
            }
            else
            {
                Hotel.Name = HotelNameDiv.InnerText.Trim();
            }
            return this;
        }


        public HotelParser ExtractAddress()
        {
            var AddressDiv = HtmlDoc.GetElementbyId("hp_address_subtitle");

            if (AddressDiv == null || AddressDiv.InnerText.Trim() == "")
            {
                Errors.Add("couldn't find hotel address");
                return this;
            }
            else
            {
                Hotel.Address = AddressDiv.InnerText.Trim();
            }
            return this;
        }

        public HotelParser ExtractClassification()
        {
            var iElement = HtmlDoc.GetElementbyId("wrap-hotelpage-top").Descendants()
                //.Where(n => n.NodeType == HtmlNodeType.Element)
                .Where(e => e.Name == "i" && e.GetAttributeValue("class", "").Contains("ratings_stars_")).FirstOrDefault();
            if (iElement != null)
            {
                string classAttributeValue = iElement.GetAttributeValue("class", "");

                string classification = RegexHelper.getFirstMatchValueOrNull(classAttributeValue, @"ratings_stars_[12345]");
                if (classification == null)
                {
                    Errors.Add("couldn't find classification");
                    return this;
                }
                else
                {
                    Hotel.Classification = (int)Char.GetNumericValue(classification[classification.Length - 1]);
                }
            }
            return this;
        }

        public HotelParser ExtractReviewPoints()
        {
            var PinkDiv = HtmlDoc.GetElementbyId("js--hp-gallery-scorecard");
            string ReviewPointsVal = RegexHelper.getFirstMatchValueOrNull(PinkDiv.InnerText, @"[0-9]*\.?[0-9]*\/10");
            if (ReviewPointsVal == null || ReviewPointsVal == "")
            {
                Errors.Add("couldn't find Review Points");
                return this;
            }
            else
            {
                Hotel.ReviewPoints = float.Parse(ReviewPointsVal.Replace("/10", ""));
            }
            return this;
        }

        public HotelParser ExtractNumberOfReviews()
        {
            var PinkDiv = HtmlDoc.GetElementbyId("js--hp-gallery-scorecard");
            string NumberOfReviews = RegexHelper.getFirstMatchValueOrNull(PinkDiv.InnerText, @"[1234567890,]+ reviews");
            
            if (NumberOfReviews == null || NumberOfReviews.Trim() == "")
            {
                Errors.Add("couldn't find Number Of Reviews");
                return this;
            }
            else
            {
                Hotel.NumberOfReviews = Int32.Parse(NumberOfReviews.Replace(" reviews", "").Replace(",", ""));
            }
            return this;
        }

        public HotelParser ExtractDescription()
        {
            var DescDiv = HtmlDoc.GetElementbyId("summary");

            if (DescDiv == null || DescDiv.InnerText.Trim() == "")
            {
                Errors.Add("couldn't find hotel description");
                return this;
            }
            else 
            {
                DescDiv.RemoveChild(DescDiv.ChildNodes[1]);
                string DescStr = DescDiv.ParentNode.InnerText;
                Hotel.Description = Regex.Replace(DescStr, @"(\n){2,}", "\n").Trim();
            }
            return this;
        }

        public HotelParser ExtractRoomCategories()
        {
            var RoomCategoriesTable = HtmlDoc.GetElementbyId("maxotel_rooms");
            if (RoomCategoriesTable == null || RoomCategoriesTable.InnerText.Trim() == "")
            {
                Errors.Add("couldn't find hotel room categories");
                return this;
            }
            else
            {
                Hotel.RoomCategories = new List<string>();
                var TableBody= RoomCategoriesTable.Descendants("tbody").FirstOrDefault();
                if (TableBody == null || TableBody.InnerText.Trim() == "")
                {
                    Errors.Add("couldn't find hotel room categories");
                    return this;
                }
                foreach (var row in TableBody.Descendants("tr"))
                {
                    if (row.ChildNodes.Count > 3)
                    {
                          Hotel.RoomCategories.Add(row.ChildNodes[3].InnerText.Trim());
                    }
                }
            }
            return this;
        }

        public HotelParser ExtractAlternativeHotels()
        {
            var YellowDiv = HtmlDoc.GetElementbyId("althotelsRow");

            if (YellowDiv == null || YellowDiv.InnerText.Trim() == "")
            {
                Errors.Add("couldn't find alternative hotels");
                return this;
            }
            else
            {
                Hotel.AlternativeHotels = new List<Hotel>();
                var alternatives = YellowDiv.Descendants("a").Where(x => x.Attributes["class"].Value == "althotel_link");
                foreach (var a in alternatives)
                {
                    System.Uri uri = new Uri(a.Attributes["href"].Value);
                    string fixedUri = uri.AbsoluteUri.Replace(uri.Query, string.Empty);
                    Hotel.AlternativeHotels.Add(new Hotel() { Name = a.InnerText.Trim(), BookingPageUrl = fixedUri });
                }

            }
            return this;
        }
        public bool IsValid() => Errors.Count == 0;
    }
}

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
        public bool RequestSucceeded = false;
        private readonly HtmlDocument htmlDoc;
        public readonly Hotel Hotel = new Hotel();
        public ICollection<string> Errors = new List<string>();

        public HotelParser()
        {
            htmlDoc = new HtmlDocument();
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
                htmlDoc.LoadHtml(text);
                RequestSucceeded = true;
            }
            catch (IOException e)
            {
                RequestSucceeded = false;
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
            var hotelNameDiv = htmlDoc.GetElementbyId("hp_hotel_name");
            if (hotelNameDiv == null || hotelNameDiv.InnerText.Trim() == "")
            {
                Errors.Add("couldn't find hotel name");
                return this;
            }
            else
            {
                Hotel.Name = hotelNameDiv.InnerText.Trim();
            }
            return this;
        }

        public HotelParser ExtractAddress()
        {
            var addressDiv = htmlDoc.GetElementbyId("hp_address_subtitle");

            if (addressDiv == null || addressDiv.InnerText.Trim() == "")
            {
                Errors.Add("couldn't find hotel address");
                return this;
            }
            else
            {
                Hotel.Address = addressDiv.InnerText.Trim();
            }
            return this;
        }

        public HotelParser ExtractClassification()
        {
            var iElement = htmlDoc.GetElementbyId("wrap-hotelpage-top").Descendants()
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
            var reviewPointsDiv = htmlDoc.GetElementbyId("js--hp-gallery-scorecard");
            string reviewPointsVal = RegexHelper.getFirstMatchValueOrNull(reviewPointsDiv.InnerText, @"[0-9]*\.?[0-9]*\/10");
            if (reviewPointsVal == null || reviewPointsVal == "")
            {
                Errors.Add("couldn't find Review Points");
                return this;
            }
            else
            {
                float reviewPointsValFloat;
                if (float.TryParse(reviewPointsVal.Replace("/10", ""), out reviewPointsValFloat))
                {
                    Hotel.ReviewPoints = reviewPointsValFloat;
                }
                else
                {
                    Errors.Add("couldn't find Review Points");
                    return this;
                }
            }
            return this;
        }

        public HotelParser ExtractNumberOfReviews()
        {
            var numberOfReviewsDiv = htmlDoc.GetElementbyId("js--hp-gallery-scorecard");
            string numberOfReviews = RegexHelper.getFirstMatchValueOrNull(numberOfReviewsDiv.InnerText, @"[1234567890,]+ reviews");
            
            if (numberOfReviews == null || numberOfReviews.Trim() == "")
            {
                Errors.Add("couldn't find Number Of Reviews");
                return this;
            }
            else
            {
                int numberOfReviewsVal;
                if (Int32.TryParse(numberOfReviews.Replace(" reviews", "").Replace(",", ""), out numberOfReviewsVal))
                {
                    Hotel.NumberOfReviews = numberOfReviewsVal;
                }
                else
                {
                    Errors.Add("couldn't find Number Of Reviews");
                    return this;
                }
            }
            return this;
        }

        public HotelParser ExtractDescription()
        {
            var descDiv = htmlDoc.GetElementbyId("summary");

            if (descDiv == null || descDiv.InnerText.Trim() == "")
            {
                Errors.Add("couldn't find hotel description");
                return this;
            }
            else 
            {
                descDiv.RemoveChild(descDiv.ChildNodes[1]);
                string descStr = descDiv.ParentNode.InnerText;
                Hotel.Description = Regex.Replace(descStr, @"(\n){2,}", "\n").Trim();
            }
            return this;
        }

        public HotelParser ExtractRoomCategories()
        {
            var roomCategoriesTableBody = htmlDoc.GetElementbyId("maxotel_rooms").Descendants("tbody").FirstOrDefault();
            if (roomCategoriesTableBody == null || roomCategoriesTableBody.InnerText.Trim() == "")
            {
                Errors.Add("couldn't find hotel room categories");
                return this;
            }
            else
            {
                Hotel.RoomCategories = new List<string>();
                foreach (var row in roomCategoriesTableBody.Descendants("tr"))
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
            var alternativeHotelsDiv = htmlDoc.GetElementbyId("althotelsRow");

            if (alternativeHotelsDiv == null || alternativeHotelsDiv.InnerText.Trim() == "")
            {
                Errors.Add("couldn't find alternative hotels");
                return this;
            }
            else
            {
                Hotel.AlternativeHotels = new List<Hotel>();
                var alternatives = alternativeHotelsDiv.Descendants("a").Where(x => x.Attributes["class"].Value == "althotel_link");
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
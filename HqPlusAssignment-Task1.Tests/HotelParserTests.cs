using NUnit.Framework;
using HqPlusAssignment_Task1;

namespace HqPlusAssignment_Task1.Tests
{
    [TestFixture]
    public class HotelParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public void ParserShouldBeValidWithAValidDocumentUrl(string documentUrl)
        {
            HotelParser hotelParser = new HotelParser();
            hotelParser.fetchHtml(documentUrl);

            if (hotelParser.RequestSucceeded)
            {
                hotelParser.ExtractHotelData();
                Assert.IsTrue(hotelParser.IsValid());
            }
        }

    }
}
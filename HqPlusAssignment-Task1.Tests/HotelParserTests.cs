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
        public void Should_ExtractData_When_Document_Is_Valid(string documentUrl)
        {
            HotelParser hotelParser = new HotelParser();
            hotelParser.fetchHtml(documentUrl);

            if (hotelParser.RequestSucceeded)
            {
                hotelParser.ExtractHotelData();
                Assert.IsTrue(hotelParser.IsValid());
            }
        }

        [TestCase("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public void Should_ExtractName_When_Document_Is_Valid(string documentUrl)
        {
            HotelParser hotelParser = new HotelParser();
            hotelParser.fetchHtml(documentUrl);

            if (hotelParser.RequestSucceeded)
            {
                hotelParser.ExtractName();
                Assert.IsTrue(hotelParser.IsValid());
            }
        }

        [TestCase("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public void Should_ExtractAddress_When_Document_Is_Valid(string documentUrl)
        {
            HotelParser hotelParser = new HotelParser();
            hotelParser.fetchHtml(documentUrl);

            if (hotelParser.RequestSucceeded)
            {
                hotelParser.ExtractAddress();
                Assert.IsTrue(hotelParser.IsValid());
            }
        }

        [TestCase("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public void Should_ExtractClassification_When_Document_Is_Valid(string documentUrl)
        {
            HotelParser hotelParser = new HotelParser();
            hotelParser.fetchHtml(documentUrl);

            if (hotelParser.RequestSucceeded)
            {
                hotelParser.ExtractClassification();
                Assert.IsTrue(hotelParser.IsValid());
            }
        }

        [TestCase("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public void Should_ExtractReviewPoints_When_Document_Is_Valid(string documentUrl)
        {
            HotelParser hotelParser = new HotelParser();
            hotelParser.fetchHtml(documentUrl);

            if (hotelParser.RequestSucceeded)
            {
                hotelParser.ExtractReviewPoints();
                Assert.IsTrue(hotelParser.IsValid());
            }
        }

        [TestCase("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public void Should_ExtractNumberOfReviews_When_Document_Is_Valid(string documentUrl)
        {
            HotelParser hotelParser = new HotelParser();
            hotelParser.fetchHtml(documentUrl);

            if (hotelParser.RequestSucceeded)
            {
                hotelParser.ExtractNumberOfReviews();
                Assert.IsTrue(hotelParser.IsValid());
            }
        }

        [TestCase("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public void Should_ExtractDescription_When_Document_Is_Valid(string documentUrl)
        {
            HotelParser hotelParser = new HotelParser();
            hotelParser.fetchHtml(documentUrl);

            if (hotelParser.RequestSucceeded)
            {
                hotelParser.ExtractDescription();
                Assert.IsTrue(hotelParser.IsValid());
            }
        }

        [TestCase("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public void Should_ExtractRoomCategories_When_Document_Is_Valid(string documentUrl)
        {
            HotelParser hotelParser = new HotelParser();
            hotelParser.fetchHtml(documentUrl);

            if (hotelParser.RequestSucceeded)
            {
                hotelParser.ExtractRoomCategories();
                Assert.IsTrue(hotelParser.IsValid());
            }
        }

        [TestCase("task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public void Should_ExtractAlternativeHotels_When_Document_Is_Valid(string documentUrl)
        {
            HotelParser hotelParser = new HotelParser();
            hotelParser.fetchHtml(documentUrl);

            if (hotelParser.RequestSucceeded)
            {
                hotelParser.ExtractAlternativeHotels();
                Assert.IsTrue(hotelParser.IsValid());
            }
        }

        //Should_FailTo_ExtractData_When_Document_Is_Invalid
        //Should_FailTo_ExtractName_When_Document_Is_Invalid
        //Should_FailTo_ExtractAddress_When_Document_Is_Invalid
        //Should_FailTo_ExtractClassification_When_Document_Is_Invalid
        //Should_FailTo_ExtractReviewPoints_When_Document_Is_Invalid
        //Should_FailTo_ExtractNumberOfReviews_When_Document_Is_Invalid
        //Should_FailTo_ExtractDescription_When_Document_Is_Invalid
        //Should_FailTo_ExtractRoomCategories_When_Document_Is_Invalid
        //Should_FailTo_ExtractAlternativeHotels_When_Document_Is_Invalid

    }
}
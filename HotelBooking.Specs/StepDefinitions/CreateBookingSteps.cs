using HotelBooking.Core;
using Moq;
using TechTalk.SpecFlow;
using Xunit;

namespace HotelBooking.Specs.StepDefinitions
{
    [Binding]
    public class CreateBookingSteps
    {
        private BookingManager bookingManager;
        private Mock<IRepository<Booking>> bookingMock;
        private Mock<IRepository<Room>> roomMock;
        DateTime _startDate, _endDate;
        bool bookingResult;

        public CreateBookingSteps()
        {
            var rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };
            var bookings = new List<Booking>
            {
                new Booking { StartDate=DateTime.Now.AddDays(5), EndDate=DateTime.Now.AddDays(10), RoomId=1, IsActive=true},
                new Booking { StartDate=DateTime.Now.AddDays(11), EndDate=DateTime.Now.AddDays(15), RoomId=1, IsActive=true},
                new Booking { StartDate=DateTime.Parse("02-12-2023"), EndDate=DateTime.Parse("07-12-2023"), RoomId=1, IsActive=true},
                new Booking { StartDate=DateTime.Now.AddDays(5), EndDate=DateTime.Now.AddDays(10), RoomId=2, IsActive=true},
                new Booking { StartDate=DateTime.Now.AddDays(11), EndDate=DateTime.Now.AddDays(15), RoomId=2, IsActive=true},
                new Booking { StartDate=DateTime.Parse("02-12-2023"), EndDate=DateTime.Parse("07-12-2023"), RoomId=2, IsActive=true},
            };

            // Create fake Repositories. 
            roomMock = new Mock<IRepository<Room>>();
            bookingMock = new Mock<IRepository<Booking>>();

            // Implement fake GetAll() method.
            roomMock.Setup(x => x.GetAll()).Returns(rooms);
            bookingMock.Setup(x => x.GetAll()).Returns(bookings);

            bookingManager = new BookingManager(bookingMock.Object, roomMock.Object);
        }

        [Scope(Tag = "CreateBooking_StartDateBeforeOccupied_EndDateInOccupied")]

        [Given(@"I create a booking with startdate (.*)")]
        public void GivenICreateABookingWithBefore(DateTime startDate)
        {
            _startDate = startDate;
        }

        [Given(@"The (.*) between occupied Start date and End date")]
        public void GivenTheBetweenAnd(DateTime endDate)
        {
            _endDate = endDate;
        }

        [When(@"I create the booking")]
        public void WhenICreateTheBooking()
        {
            Booking b = new Booking
            {
                StartDate = _startDate,
                EndDate = _endDate
            };

            bookingResult = bookingManager.CreateBooking(b);
        }

        [Then(@"The result should be False")]
        public void ThenTheResultShouldBe()
        {
            Assert.False(bookingResult);
        }

        //Scenario 2

        [Scope(Tag = "CreateBooking_StartDateBeforeOccupied_EndDateAfterOccupied")]

        [Given(@"I create a booking with startdate (.*)")]
        public void GivenICreateABookingWith(DateTime startDate)
        {
            _startDate = startDate;
        }

        [Given(@"The following End date (.*)")]
        public void GivenTheFollowingEndDate(DateTime endDate)
        {
            _endDate = endDate;
        }

        [When(@"I create booking")]
        public void WhenICreateBooking()
        {
            Booking b = new Booking
            {
                StartDate = _startDate,
                EndDate = _endDate
            };

            bookingResult = bookingManager.CreateBooking(b);
        }

        [Then(@"The result is False")]
        public void ThenTheResultIsFalse()
        {
            Assert.False(bookingResult);
        }

    }
}
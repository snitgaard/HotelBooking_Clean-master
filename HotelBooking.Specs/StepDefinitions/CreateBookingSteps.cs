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
                new Booking { StartDate=DateTime.Parse("2023-12-02"), EndDate=DateTime.Parse("2023-12-07"), RoomId=1, IsActive=true},
                new Booking { StartDate=DateTime.Now.AddDays(5), EndDate=DateTime.Now.AddDays(10), RoomId=2, IsActive=true},
                new Booking { StartDate=DateTime.Now.AddDays(11), EndDate=DateTime.Now.AddDays(15), RoomId=2, IsActive=true},
                new Booking { StartDate=DateTime.Parse("2023-12-02"), EndDate=DateTime.Parse("2023-12-07"), RoomId=2, IsActive=true},
            };

            // Create fake Repositories. 
            roomMock = new Mock<IRepository<Room>>();
            bookingMock = new Mock<IRepository<Booking>>();

            // Implement fake GetAll() method.
            roomMock.Setup(x => x.GetAll()).Returns(rooms);
            bookingMock.Setup(x => x.GetAll()).Returns(bookings);

            bookingManager = new BookingManager(bookingMock.Object, roomMock.Object);
        }

        // Scenario 1
        [Given(@"I create a booking with startdate '(.*)'")]
        public void GivenICreateABookingWithBefore(string startDate)
        {
            _startDate = DateTime.Parse(startDate);
        }

        [Given(@"The following EndDate '(.*)'")]
        public void GivenTheBetweenAnd(string endDate)
        {
            _endDate = DateTime.Parse(endDate);
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

        [Given(@"The following StartDate '(.*)'")]
        public void GivenTheFollowingStartDate(string startDate)
        {
            _startDate = DateTime.Parse(startDate);
        }

        [Given(@"The EndDate '(.*)'")]
        public void GivenTheEndDate(string endDate)
        {
            _endDate = DateTime.Parse(endDate);
        }

        [When(@"I create a booking")]
        public void WhenICreateABooking()
        {
            Booking b = new Booking
            {
                StartDate = _startDate,
                EndDate = _endDate
            };

            bookingResult = bookingManager.CreateBooking(b);
        }

        [Then(@"The result will be True")]
        public void TheResultWillBeTrue()
        {
            Assert.True(bookingResult);
        }





        //Other examples

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
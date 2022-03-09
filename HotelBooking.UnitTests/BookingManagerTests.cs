using System;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;
        private Mock<IRepository<Booking>> bookingMock;
        private Mock<IRepository<Room>> roomMock;

        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);

            bookingMock = new Mock<IRepository<Booking>>();
            roomMock = new Mock<IRepository<Room>>();
        }



        [InlineData(1, "2024-03-02", "2024-03-09", true)]
        [Theory]
        public void CreateValidBookingNotExist(int id, string startDate, string endDate, bool isActive)
        {
            IRepository<Booking> bookingRepo = bookingMock.Object;
            IRepository<Room> roomRepo = roomMock.Object;

            BookingManager manager = new BookingManager(bookingRepo, roomRepo);

            Booking b = new Booking()
            {
                Id = id,
                StartDate = DateTime.Parse(startDate),
                EndDate = DateTime.Parse(endDate),
                IsActive = isActive
            };
            manager.CreateBooking(b);
            bookingMock.Verify(repo => repo.Add(It.Is<Booking>((bo => bo == b))), Times.Once);
        }

        [InlineData(1, "01-03-2022", "01-03-2022")]
        [InlineData(1, "01-03-2022", "27-02-2022")]
        [Theory]
        public void CreateInvalidBookingExpectArgumentException(int id, string startDate, string endDate)
        {
            Booking b = new Booking()
            {
                Id = id,
                StartDate = DateTime.Parse(startDate),
                EndDate = DateTime.Parse(endDate),
            };
            var ex = Assert.Throws<ArgumentException>(() => bookingManager.CreateBooking(b));
            Assert.Equal("The start date cannot be in the past or later than the end date.", ex.Message);
            bookingMock.Verify(repo => repo.Add(It.Is<Booking>(bo => bo == b)), Times.Never);
        }

        /*
        [Fact]
        public void CreateBookingExistingBookingExpectInvalidOperationException()
        {
            IRepository<Booking> bookingRepo = bookingMock.Object;
            IRepository<Room> roomRepo = roomMock.Object;

            BookingManager manager = new BookingManager(bookingRepo, roomRepo);

            Booking booking = new Booking()
            {
                Id = 1,
                CustomerId = 1,
                StartDate = DateTime.Parse("06-03-2022"),
                EndDate = DateTime.Parse("08-03-2022"),
                RoomId = 1
            };

            bookingMock.Setup(repo => repo.Get(It.Is<int>(x => x == booking.Id))).Returns(() => booking);
            var bookingEx = Assert.Throws<InvalidOperationException>(() => manager.CreateBooking(booking));
            Assert.Equal("Booking already exists.", bookingEx.Message);
            bookingMock.Verify(repo => repo.Add(It.Is<Booking>(b => b == booking)), Times.Never);
        }
        */

        public void FindNotAvailableRoomExpectArgumentException()
        {

        }

        [Fact]
        public void FindValidAvailableRoom()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        public void GetInvalidFullyOccupiedDatesExpectArgumentException()
        {

        }

        public void GetValidFullyOcupiedDates()
        {

        }

        /*
        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            // Arrange
            DateTime date = DateTime.Today;

            // Act
            Action act = () => bookingManager.FindAvailableRoom(date, date);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }
        */
    }
}

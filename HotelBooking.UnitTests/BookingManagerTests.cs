using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private BookingManager bookingManager;
        private Mock<IRepository<Booking>> bookingMock;
        private Mock<IRepository<Room>> roomMock;

        public BookingManagerTests()
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

        [Fact]
        public void CreateValidBookingNotExist()
        {
            // Arrange
            Booking b = new Booking()
            {
                Id = 1,
                StartDate = DateTime.Parse("01-03-2028"),
                EndDate = DateTime.Parse("04-03-2028"),
            };
            // Act
            var booking = bookingManager.CreateBooking(b);
            // Assert
            Assert.True(booking);
        }

        [InlineData(1, "04-03-2022", "02-03-2022")] //End date before start date
        [InlineData(1, "01-02-2022", "08-02-2022")] //Start and end date in the past
        [InlineData(1, "01-02-2022", "04-04-2022")] //Start date in the past and end date after today
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

        [InlineData("01-12-2023", "08-12-2023")]
        [InlineData("01-12-2023", "04-12-2023")]
        [InlineData("02-12-2023", "04-12-2023")]
        [InlineData("02-12-2023", "08-12-2023")]
        [Theory]
        public void CreateBookingOnFullyOccupiedDatesExpectFalse(string startDate, string endDate)
        {
            Booking b = new Booking()
            {
                StartDate = DateTime.Parse(startDate),
                EndDate = DateTime.Parse(endDate),
            };
            Assert.False(bookingManager.CreateBooking(b));
            bookingMock.Verify(repo => repo.Add(It.Is<Booking>(bo => bo == b)), Times.Never);
        }

        [Fact]
        public void CreateBookingBeforeFullyOccupiedDatesExpectTrue()
        {
            Booking b = new Booking()
            {
                Id = 2,
                StartDate = DateTime.Parse("10-11-2023"),
                EndDate = DateTime.Parse("12-11-2023"),
            };
            Assert.True(bookingManager.CreateBooking(b));
            bookingMock.Verify(repo => repo.Add(It.Is<Booking>(bo => bo == b)), Times.Once);
        }

        [Fact]
        public void CreateBookingAfterFullyOccupiedDatesExpectTrue()
        {
            Booking b = new Booking()
            {
                Id = 2,
                StartDate = DateTime.Parse("9-12-2023"),
                EndDate = DateTime.Parse("12-12-2023"),
            };
            Assert.True(bookingManager.CreateBooking(b));
            bookingMock.Verify(repo => repo.Add(It.Is<Booking>(bo => bo == b)), Times.Once);
        }

        [Fact]
        public void FindValidAvailableRoom()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(30);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        
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
    }
}

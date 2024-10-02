using Caesar.Core.DTOs;
using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using Caesar.Core.Services;
using Moq;

namespace Caesar.Tests;

public class ReservationServiceTests
{
    [Fact]
    public async Task CreateReservationAsync_ShouldReturnReservationDto()
    {
        // Arrange
        var mockRepo = new Mock<IReservationRepository>();
        mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Reservation>()))
            .ReturnsAsync((Reservation r) => r);

        var service = new ReservationService(mockRepo.Object);
        var reservationDto = new ReservationDto
        {
            ReservationDate = DateTime.Today.AddDays(1),
            ReservationTime = new TimeSpan(18, 0, 0),
            NumberOfGuests = 2,
        };

        // Act
        var result = await service.CreateReservationAsync(reservationDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reservationDto.ReservationDate, result.ReservationDate);
        Assert.Equal(reservationDto.NumberOfGuests, result.NumberOfGuests);
    }
}

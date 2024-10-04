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


    }
}

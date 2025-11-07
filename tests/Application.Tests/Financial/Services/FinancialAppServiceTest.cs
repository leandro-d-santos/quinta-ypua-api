using Application.Financial.Services;
using Application.Financial.ViewModels;
using Domain.Common.Data;
using Domain.Financial.Repositories;
using Domain.Guests.Models;
using Domain.Guests.Repositories;
using Domain.Reservations.Models;
using Domain.Reservations.Repositories;
using Domain.Rooms.Models;
using Domain.Rooms.Repositories;
using Moq;

namespace Application.Tests.Financials.Services;

public class FinancialAppServiceTest
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<IFinancialRepository> _financialRepositoryMock;
    private readonly Mock<IGuestRepository> _guestRepositoryMock;
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly FinancialService _financialService;

    public FinancialAppServiceTest()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _financialRepositoryMock = new Mock<IFinancialRepository>();
        _guestRepositoryMock = new Mock<IGuestRepository>();
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _financialService = new FinancialService(
            _reservationRepositoryMock.Object,
            _financialRepositoryMock.Object,
            _guestRepositoryMock.Object,
            _roomRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task FindAllAsync_Should_Return_Mapped_ViewModels()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var reservations = new List<Reservation>
            {
                new() { Id = reservationId, GuestId = Guid.NewGuid(), RoomId = Guid.NewGuid() }
            };
        var financials = new List<Domain.Financial.Models.Financial>
            {
                new() { Id = reservationId, ReservationValue = 100, Payment = "PIX", AdditionalValue = 10, Status = "Pago" }
            };
        var guests = new List<Guest> { new() { Id = reservations[0].GuestId, Name = "John Doe" } };
        var rooms = new List<Room> { new() { Id = reservations[0].RoomId, Description = "Suite 1" } };

        _reservationRepositoryMock.Setup(r => r.FindAll()).Returns(reservations);
        _financialRepositoryMock.Setup(f => f.FindAll()).Returns(financials);
        _guestRepositoryMock.Setup(g => g.FindAll()).Returns(guests);
        _roomRepositoryMock.Setup(r => r.FindAll()).Returns(rooms);

        // Act
        var result = await _financialService.FindAllAsync();

        // Assert
        Assert.Single(result);
        var vm = result.First();
        Assert.Equal("John Doe", vm.GuestName);
        Assert.Equal("Suite 1", vm.RoomName);
        Assert.Equal(100, vm.ReservationValue);
        Assert.Equal("Pago", vm.Status);
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Null_When_Reservation_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        _reservationRepositoryMock.Setup(r => r.FindById(id)).Returns((Reservation?)null);

        // Act
        var result = await _financialService.FindByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_FinancialViewModel_When_Exists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var reservation = new Reservation { Id = id, GuestId = Guid.NewGuid(), RoomId = Guid.NewGuid() };
        var financial = new Domain.Financial.Models.Financial { Id = id, Payment = "Cartão", Status = "Pago", ReservationValue = 300 };
        var guests = new List<Guest> { new() { Id = reservation.GuestId, Name = "Maria" } };
        var rooms = new List<Room> { new() { Id = reservation.RoomId, Description = "Suite Master" } };

        _reservationRepositoryMock.Setup(r => r.FindById(id)).Returns(reservation);
        _financialRepositoryMock.Setup(f => f.FindById(id)).Returns(financial);
        _guestRepositoryMock.Setup(g => g.FindAll()).Returns(guests);
        _roomRepositoryMock.Setup(r => r.FindAll()).Returns(rooms);

        // Act
        var result = await _financialService.FindByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Maria", result!.GuestName);
        Assert.Equal("Suite Master", result.RoomName);
        Assert.Equal(300, result.ReservationValue);
        Assert.Equal("Pago", result.Status);
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_Update_And_Complete()
    {
        // Arrange
        var vm = new FinancialViewModel
        {
            Id = Guid.NewGuid(),
            Payment = "PIX",
            Status = "Pago",
            ReservationValue = 150,
            AdditionalValue = 20
        };

        // Act
        var result = await _financialService.UpdateAsync(vm);

        // Assert
        _financialRepositoryMock.Verify(f => f.Update(It.IsAny<Domain.Financial.Models.Financial>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task CheckOutAsync_Should_Call_CheckOut_And_Complete()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var result = await _financialService.CheckOutAsync(id);

        // Assert
        _financialRepositoryMock.Verify(f => f.CheckOutAsync(id), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }
}
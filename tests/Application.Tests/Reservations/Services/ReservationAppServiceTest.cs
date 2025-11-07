using Application.Reservations.Services;
using Application.Reservations.ViewModels;
using Domain.Common.Data;
using Domain.Financial.Repositories;
using Domain.Reservations.Models;
using Domain.Reservations.Repositories;
using Domain.Rooms.Models;
using Domain.Rooms.Repositories;
using Moq;

namespace Application.Tests.Reservations.Services;

public class ReservationAppServiceTest
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<IFinancialRepository> _financialRepositoryMock;
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ReservationService _reservationService;

    public ReservationAppServiceTest()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _financialRepositoryMock = new Mock<IFinancialRepository>();
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _reservationService = new ReservationService(
            _reservationRepositoryMock.Object,
            _financialRepositoryMock.Object,
            _roomRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Reservation_And_Financial_When_Valid()
    {
        // Arrange
        var reservationVm = new ReservationViewModel
        {
            Id = Guid.NewGuid(),
            RoomId = Guid.NewGuid(),
            GuestId = Guid.NewGuid(),
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(2),
            NumberOfAdults = 2,
            NumberOfChildren = 1
        };

        _reservationRepositoryMock
            .Setup(r => r.ExistsInPeriodByRoom(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(false);

        // Act
        var result = await _reservationService.CreateAsync(reservationVm);

        // Assert
        Assert.True(result);
        _reservationRepositoryMock.Verify(r => r.Add(It.IsAny<Reservation>()), Times.Once);
        _financialRepositoryMock.Verify(f => f.Create(It.IsAny<Domain.Financial.Models.Financial>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Period_Is_Invalid()
    {
        // Arrange
        var reservationVm = new ReservationViewModel
        {
            CheckIn = DateTime.Today.AddDays(3),
            CheckOut = DateTime.Today.AddDays(1)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _reservationService.CreateAsync(reservationVm));
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Room_Already_Reserved()
    {
        // Arrange
        var reservationVm = new ReservationViewModel
        {
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(2),
            RoomId = Guid.NewGuid()
        };

        _reservationRepositoryMock
            .Setup(r => r.ExistsInPeriodByRoom(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _reservationService.CreateAsync(reservationVm));
    }

    [Fact]
    public async Task FindAllAsync_Should_Return_All_Reservations()
    {
        // Arrange
        var reservations = new List<Reservation>
            {
                new() { Id = Guid.NewGuid(), CheckIn = DateTime.Today, CheckOut = DateTime.Today.AddDays(2) },
                new() { Id = Guid.NewGuid(), CheckIn = DateTime.Today, CheckOut = DateTime.Today.AddDays(1) }
            };
        _reservationRepositoryMock.Setup(r => r.FindAll()).Returns(reservations);

        // Act
        var result = await _reservationService.FindAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Reservation_When_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        var reservation = new Reservation { Id = id, NumberOfAdults = 2 };
        _reservationRepositoryMock.Setup(r => r.FindById(id)).Returns(reservation);

        // Act
        var result = await _reservationService.FindByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result!.NumberOfAdults);
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        _reservationRepositoryMock.Setup(r => r.FindById(id)).Returns((Reservation?)null);

        // Act
        var result = await _reservationService.FindByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_And_Complete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var reservation = new Reservation { Id = id };
        var reservationVm = new ReservationViewModel
        {
            Id = id,
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(1),
            NumberOfAdults = 2,
            NumberOfChildren = 0,
            RoomId = Guid.NewGuid(),
            GuestId = Guid.NewGuid()
        };

        _reservationRepositoryMock.Setup(r => r.FindById(id)).Returns(reservation);

        // Act
        var result = await _reservationService.UpdateAsync(reservationVm);

        // Assert
        Assert.True(result);
        _reservationRepositoryMock.Verify(r => r.Update(It.Is<Reservation>(res =>
            res.NumberOfAdults == 2 &&
            res.NumberOfChildren == 0
        )), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Reservation_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        var reservationVm = new ReservationViewModel { Id = id };
        _reservationRepositoryMock.Setup(r => r.FindById(id)).Returns((Reservation?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _reservationService.UpdateAsync(reservationVm));
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_And_Complete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var reservation = new Reservation { Id = id };
        _reservationRepositoryMock.Setup(r => r.FindById(id)).Returns(reservation);

        // Act
        var result = await _reservationService.RemoveAsync(id);

        // Assert
        Assert.True(result);
        _reservationRepositoryMock.Verify(r => r.Remove(reservation), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_Should_Throw_When_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        _reservationRepositoryMock.Setup(r => r.FindById(id)).Returns((Reservation?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _reservationService.RemoveAsync(id));
    }

    [Fact]
    public async Task GetCurrentRoomsStatusAsync_Should_Return_Result_From_Repository()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var rooms = new List<Room> { new Room { Id = roomId, Number = "101", Description = "Suite" } };

        var expectedStatus = new List<RoomStatusDto>
    {
        new RoomStatusDto
        {
            RoomId = roomId,
            RoomNumber = "101",
            Descricao = "Suite",
            Status = "Ocupado",
            PeriodoReserva = "2025-11-01 a 2025-11-03",
            Hospede = "Maria"
        }
    };

        _roomRepositoryMock.Setup(r => r.FindAll()).Returns(rooms);

        _reservationRepositoryMock
            .Setup(r => r.GetCurrentRoomsStatusAsync(
                It.Is<List<string>>(ids => ids.Count == 1 && ids[0] == roomId.ToString())))
            .ReturnsAsync(expectedStatus);

        // Act
        var result = await _reservationService.GetCurrentRoomsStatusAsync();

        // Assert
        Assert.Single(result);
        var dto = result.First();
        Assert.Equal(roomId, dto.RoomId);
        Assert.Equal("101", dto.RoomNumber);
        Assert.Equal("Suite", dto.Descricao);
        Assert.Equal("Ocupado", dto.Status);
        Assert.Equal("2025-11-01 a 2025-11-03", dto.PeriodoReserva);
        Assert.Equal("Maria", dto.Hospede);
    }
}
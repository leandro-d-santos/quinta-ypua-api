using Application.Guests.Services;
using Application.Guests.ViewModels;
using Domain.Common.Data;
using Domain.Guests.Models;
using Domain.Guests.Repositories;
using Moq;

namespace Application.Tests.Guests.Services;

public class GuestAppServiceTest
{
    private readonly Mock<IGuestRepository> _guestRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GuestService _guestService;

    public GuestAppServiceTest()
    {
        _guestRepositoryMock = new Mock<IGuestRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _guestService = new GuestService(_guestRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Guest_And_Complete()
    {
        // Arrange
        var guestVm = new GuestViewModel
        {
            Name = "John Doe",
            Email = "john@email.com",
            CPF = "12345678900",
            PhoneNumber = "999999999"
        };

        // Act
        var result = await _guestService.CreateAsync(guestVm);

        // Assert
        _guestRepositoryMock.Verify(r => r.Add(It.IsAny<Guest>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task FindAllAsync_Should_Return_All_Guests_As_ViewModels()
    {
        // Arrange
        var guests = new List<Guest>
        {
            new Guest { Id = Guid.NewGuid(), Name = "John", Email = "j@x.com", CPF = "1", PhoneNumber = "11" },
            new Guest { Id = Guid.NewGuid(), Name = "Mary", Email = "m@x.com", CPF = "2", PhoneNumber = "22" }
        };

        _guestRepositoryMock.Setup(r => r.FindAll()).Returns(guests);

        // Act
        var result = await _guestService.FindAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, g => g.Name == "John");
        Assert.Contains(result, g => g.Name == "Mary");
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Guest_When_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        var guest = new Guest { Id = id, Name = "Ana" };
        _guestRepositoryMock.Setup(r => r.FindById(id)).Returns(guest);

        // Act
        var result = await _guestService.FindByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Ana", result!.Name);
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        _guestRepositoryMock.Setup(r => r.FindById(id)).Returns((Guest?)null);

        // Act
        var result = await _guestService.FindByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Guest_And_Complete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var guest = new Guest { Id = id, Name = "Old", Email = "old@mail.com", CPF = "111", PhoneNumber = "000" };
        var guestVm = new GuestViewModel
        {
            Id = id,
            Name = "New",
            Email = "new@mail.com",
            CPF = "222",
            PhoneNumber = "999"
        };

        _guestRepositoryMock.Setup(r => r.FindById(id)).Returns(guest);

        // Act
        var result = await _guestService.UpdateAsync(guestVm);

        // Assert
        _guestRepositoryMock.Verify(r => r.Update(It.Is<Guest>(g =>
            g.Name == "New" &&
            g.Email == "new@mail.com" &&
            g.CPF == "222" &&
            g.PhoneNumber == "999"
        )), Times.Once);

        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Guest_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        var guestVm = new GuestViewModel { Id = id, Name = "Not Found" };
        _guestRepositoryMock.Setup(r => r.FindById(id)).Returns((Guest?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _guestService.UpdateAsync(guestVm));
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_Guest_And_Complete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var guest = new Guest { Id = id, Name = "RemoveMe" };
        _guestRepositoryMock.Setup(r => r.FindById(id)).Returns(guest);

        // Act
        var result = await _guestService.RemoveAsync(id);

        // Assert
        _guestRepositoryMock.Verify(r => r.Remove(guest), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Throw_When_Guest_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        _guestRepositoryMock.Setup(r => r.FindById(id)).Returns((Guest?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _guestService.RemoveAsync(id));
    }
}
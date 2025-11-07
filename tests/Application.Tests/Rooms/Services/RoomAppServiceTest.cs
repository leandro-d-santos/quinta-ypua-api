using Application.Rooms.Services;
using Application.Rooms.ViewModels;
using Domain.Common.Data;
using Domain.Rooms.Models;
using Domain.Rooms.Repositories;
using Moq;

namespace Application.Tests.Rooms.Services;

public class RoomAppServiceTest
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RoomService _roomService;

    public RoomAppServiceTest()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _roomService = new RoomService(_roomRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Room_And_Complete()
    {
        // Arrange
        var roomVm = new RoomViewModel
        {
            Number = "101",
            Floor = "1",
            Description = "Suite",
            Capacity = 2
        };

        // Act
        var result = await _roomService.CreateAsync(roomVm);

        // Assert
        _roomRepositoryMock.Verify(r => r.Add(It.IsAny<Room>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task FindAllAsync_Should_Return_All_Rooms_As_ViewModels()
    {
        // Arrange
        var rooms = new List<Room>
            {
                new Room { Id = Guid.NewGuid(), Number = "101", Floor = "1", Description = "A", Capacity = 2 },
                new Room { Id = Guid.NewGuid(), Number = "102", Floor = "1", Description = "B", Capacity = 4 }
            };

        _roomRepositoryMock.Setup(r => r.FindAll()).Returns(rooms);

        // Act
        var result = await _roomService.FindAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Number == "101");
        Assert.Contains(result, r => r.Number == "102");
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Room_When_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        var room = new Room { Id = id, Number = "101" };
        _roomRepositoryMock.Setup(r => r.FindById(id)).Returns(room);

        // Act
        var result = await _roomService.FindByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("101", result!.Number);
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        _roomRepositoryMock.Setup(r => r.FindById(id)).Returns((Room?)null);

        // Act
        var result = await _roomService.FindByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Room_And_Complete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var room = new Room { Id = id, Number = "101", Floor = "1", Description = "Old", Capacity = 2 };
        var roomVm = new RoomViewModel
        {
            Id = id,
            Number = "202",
            Floor = "2",
            Description = "Updated",
            Capacity = 3
        };

        _roomRepositoryMock.Setup(r => r.FindById(id)).Returns(room);

        // Act
        var result = await _roomService.UpdateAsync(roomVm);

        // Assert
        _roomRepositoryMock.Verify(r => r.Update(It.Is<Room>(x =>
            x.Number == "202" &&
            x.Floor == "2" &&
            x.Description == "Updated" &&
            x.Capacity == 3
        )), Times.Once);

        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Room_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        var roomVm = new RoomViewModel { Id = id, Number = "404" };
        _roomRepositoryMock.Setup(r => r.FindById(id)).Returns((Room?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _roomService.UpdateAsync(roomVm));
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_Room_And_Complete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var room = new Room { Id = id, Number = "101" };
        _roomRepositoryMock.Setup(r => r.FindById(id)).Returns(room);

        // Act
        var result = await _roomService.RemoveAsync(id);

        // Assert
        _roomRepositoryMock.Verify(r => r.Remove(room), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Throw_When_Room_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();
        _roomRepositoryMock.Setup(r => r.FindById(id)).Returns((Room?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _roomService.RemoveAsync(id));
    }
}
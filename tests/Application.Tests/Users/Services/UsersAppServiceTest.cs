using Application.Users.Services;
using Application.Users.ViewModels;
using Domain.Common.Data;
using Domain.Users.Models;
using Domain.Users.Repositories;
using Moq;

namespace Application.Tests.Users.Services;

public class UsersAppServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UserService _userService;

    public UsersAppServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userService = new UserService(_userRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_User_And_Complete()
    {
        // Arrange
        var userVm = new UserViewModel
        {
            UserName = "leandro",
            Name = "Leandro Santos",
            Email = "leandro@email.com",
            Password = "1234"
        };

        // Act
        var result = await _userService.CreateAsync(userVm);

        // Assert
        _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task FindAllAsync_Should_Return_All_Users_As_ViewModels()
    {
        // Arrange
        var users = new List<User>
        {
            new User { UserName = "u1", Name = "User 1", Email = "u1@test.com" },
            new User { UserName = "u2", Name = "User 2", Email = "u2@test.com" }
        };
        _userRepositoryMock.Setup(r => r.FindAll()).Returns(users);

        // Act
        var result = await _userService.FindAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.UserName == "u1");
        Assert.Contains(result, u => u.UserName == "u2");
    }

    [Fact]
    public async Task FindByUserNameAsync_Should_Return_UserViewModel_When_User_Exists()
    {
        // Arrange
        var user = new User { UserName = "leandro", Name = "Leandro", Email = "a@a.com" };
        _userRepositoryMock.Setup(r => r.FindByUserName("leandro")).Returns(user);

        // Act
        var result = await _userService.FindByUserNameAsync("leandro");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("leandro", result!.UserName);
    }

    [Fact]
    public async Task FindByUserNameAsync_Should_Return_Null_When_User_Not_Found()
    {
        // Arrange
        _userRepositoryMock.Setup(r => r.FindByUserName("x")).Returns((User?)null);

        // Act
        var result = await _userService.FindByUserNameAsync("x");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_User_And_Complete()
    {
        // Arrange
        var user = new User { UserName = "leo", Name = "Old", Email = "old@test.com" };
        var userVm = new UserViewModel { UserName = "leo", Name = "New", Email = "new@test.com" };

        _userRepositoryMock.Setup(r => r.FindByUserName("leo")).Returns(user);

        // Act
        var result = await _userService.UpdateAsync(userVm);

        // Assert
        _userRepositoryMock.Verify(r => r.Update(It.Is<User>(u => u.Name == "New" && u.Email == "new@test.com")), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_User_Not_Found()
    {
        // Arrange
        var userVm = new UserViewModel { UserName = "notfound" };
        _userRepositoryMock.Setup(r => r.FindByUserName("notfound")).Returns((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateAsync(userVm));
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_User_And_Complete()
    {
        // Arrange
        var user = new User { UserName = "leo" };
        _userRepositoryMock.Setup(r => r.FindByUserName("leo")).Returns(user);

        // Act
        var result = await _userService.RemoveAsync("leo");

        // Assert
        _userRepositoryMock.Verify(r => r.Remove(user), Times.Once);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Throw_When_User_Not_Found()
    {
        // Arrange
        _userRepositoryMock.Setup(r => r.FindByUserName("x")).Returns((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.RemoveAsync("x"));
    }
}

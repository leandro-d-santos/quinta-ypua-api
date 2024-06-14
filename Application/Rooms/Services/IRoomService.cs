using Application.Rooms.ViewModels;

namespace Application.Rooms.Services
{
    public interface IRoomService
    {
        Task<bool> CreateAsync(RoomViewModel roomViewModel);
        Task<IList<RoomViewModel>> FindAllAsync();
        Task<RoomViewModel?> FindByIdAsync(Guid id);
        Task<bool> UpdateAsync(RoomViewModel roomViewModel);
        Task<bool> RemoveAsync(Guid id);
    }
}
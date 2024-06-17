using Application.Guests.ViewModels;

namespace Application.Guests.Services
{
    public interface IGuestService
    {
        Task<bool> CreateAsync(GuestViewModel guestViewModel);
        Task<IList<GuestViewModel>> FindAllAsync();
        Task<GuestViewModel?> FindByIdAsync(Guid id);
        Task<bool> UpdateAsync(GuestViewModel guestViewModel);
        Task<bool> RemoveAsync(Guid id);
    }
}
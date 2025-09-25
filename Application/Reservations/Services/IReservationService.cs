using Application.Reservations.ViewModels;
using Domain.Reservations.Models;

namespace Application.Reservations.Services
{
    public interface IReservationService
    {
        Task<bool> CreateAsync(ReservationViewModel reservationViewModel);
        Task<IList<ReservationViewModel>> FindAllAsync();
        Task<ReservationViewModel?> FindByIdAsync(Guid id);
        Task<bool> UpdateAsync(ReservationViewModel reservationViewModel);
        Task<bool> RemoveAsync(Guid id);
        Task<List<RoomStatusDto>> GetCurrentRoomsStatusAsync();
    }
}
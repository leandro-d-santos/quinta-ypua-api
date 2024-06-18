using Application.Reservations.ViewModels;

namespace Application.Reservations.Services
{
    public interface IReservationService
    {
        Task<bool> CreateAsync(ReservationViewModel reservationViewModel);
        Task<IList<ReservationViewModel>> FindAllAsync();
        Task<ReservationViewModel?> FindByIdAsync(Guid id);
        Task<bool> UpdateAsync(ReservationViewModel reservationViewModel);
        Task<bool> RemoveAsync(Guid id);
    }
}
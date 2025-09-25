using Domain.Reservations.Models;

namespace Domain.Reservations.Repositories
{
    public interface IReservationRepository
    {
        void Add(Reservation reservation);
        IList<Reservation> FindAll();
        Reservation? FindById(Guid id);
        void Update(Reservation reservation);
        void Remove(Reservation reservation);
        bool ExistsInPeriodByRoom(Guid roomId, DateTime checkIn, DateTime checkOut);
        Task<List<RoomStatusDto>> GetCurrentRoomsStatusAsync(List<string>? roomIds = null);
    }
}
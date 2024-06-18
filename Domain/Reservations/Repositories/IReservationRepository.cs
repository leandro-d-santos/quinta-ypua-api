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
    }
}
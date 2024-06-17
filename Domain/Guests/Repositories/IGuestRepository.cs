using Domain.Guests.Models;

namespace Domain.Guests.Repositories
{
    public interface IGuestRepository
    {
        void Add(Guest guest);
        IList<Guest> FindAll();
        Guest? FindById(Guid id);
        void Update(Guest guest);
        void Remove(Guest guest);
    }
}
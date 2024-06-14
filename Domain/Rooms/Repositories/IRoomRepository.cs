using Domain.Rooms.Models;

namespace Domain.Rooms.Repositories
{
    public interface IRoomRepository
    {
        void Add(Room room);
        IList<Room> FindAll();
        Room? FindById(Guid id);
        void Update(Room room);
        void Remove(Room room);
    }
}
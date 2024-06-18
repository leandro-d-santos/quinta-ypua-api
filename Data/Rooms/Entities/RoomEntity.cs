using Data.Common.Entities;
using Data.Reservations.Entities;
using Domain.Rooms.Models;

namespace Data.Rooms.Entities
{
    public class RoomEntity : Entity
    {

        public string Id { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public ReservationEntity Reservation { get; set; } = null;

        public RoomEntity() { }

        public RoomEntity(Guid id)
        {
            Id = id.ToString();
        }

        public static RoomEntity CreateFromModel(Room room)
        {
            return new RoomEntity()
            {
                Id = room.Id.ToString(),
                Number = room.Number,
                Floor = room.Floor,
                Description = room.Description,
                Capacity = room.Capacity
            };
        }

        public override Room TransformToModel()
        {
            return new Room()
            {
                Id = new Guid(Id),
                Number = Number,
                Floor = Floor,
                Description = Description,
                Capacity = Capacity
            };
        }
    }
}
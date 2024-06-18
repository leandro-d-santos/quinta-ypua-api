using Data.Common.Entities;
using Data.Guests.Entities;
using Data.Rooms.Entities;
using Domain.Reservations.Models;

namespace Data.Reservations.Entities
{
    public class ReservationEntity : Entity
    {
        public string Id { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public string RoomId { get; set; } = string.Empty;
        public string GuestId { get; set; } = string.Empty;
        public RoomEntity Room { get; set; }
        public GuestEntity Guest { get; set; }

        public static ReservationEntity CreateFromModel(Reservation reservation)
        {
            return new ReservationEntity()
            {
                Id = reservation.Id.ToString(),
                CheckIn = reservation.CheckIn,
                CheckOut = reservation.CheckOut,
                NumberOfAdults = reservation.NumberOfAdults,
                NumberOfChildren = reservation.NumberOfChildren,
                Room = new RoomEntity(reservation.RoomId),
                Guest = new GuestEntity(reservation.GuestId),
            };
        }

        public override Reservation TransformToModel()
        {
            return new Reservation()
            {
                Id = new Guid(Id),
                CheckIn = CheckIn,
                CheckOut = CheckOut,
                NumberOfAdults = NumberOfAdults,
                NumberOfChildren = NumberOfChildren,
                RoomId = new Guid(Room.Id),
                GuestId = new Guid(Guest.Id)
            };
        }
    }
}
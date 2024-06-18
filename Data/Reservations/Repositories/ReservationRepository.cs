using Data.Context;
using Data.Guests.Entities;
using Data.Reservations.Entities;
using Data.Rooms.Entities;
using Domain.Common.Utils;
using Domain.Reservations.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Reservations.Repositories
{
    public sealed class ReservationRepository : IReservationRepository
    {
        private readonly DbConnection context;

        public ReservationRepository(DbConnection context)
        {
            this.context = context;
        }

        public void Add(Reservation reservation)
        {
            Check.ThrowIfNull(reservation, nameof(reservation));
            ReservationEntity entity = ReservationEntity.CreateFromModel(reservation);
            context.Entry(entity.Guest).State = EntityState.Unchanged;
            context.Entry(entity.Room).State = EntityState.Unchanged;
            context.Set<ReservationEntity>().Add(entity);
        }

        public IList<Reservation> FindAll()
        {
            return context.Set<ReservationEntity>()
                .Include(e => e.Room)
                .Include(e => e.Guest)
                .Select((reservation) => reservation.TransformToModel())
                .ToList();
        }

        public Reservation? FindById(Guid id)
        {
            Check.ThrowIfNull(id, nameof(id));
            ReservationEntity? reservation = FindReservationEntityById(id);
            if (reservation is null)
            {
                return null;
            }
            return reservation.TransformToModel();
        }

        public void Update(Reservation reservation)
        {
            Check.ThrowIfNull(reservation, nameof(reservation));
            ReservationEntity? entity = FindReservationEntityById(reservation.Id);
            if (entity is null)
            {
                return;
            }
            entity.CheckIn = reservation.CheckIn;
            entity.CheckOut = reservation.CheckOut;
            entity.NumberOfAdults = reservation.NumberOfAdults;
            entity.NumberOfChildren = reservation.NumberOfChildren;
            entity.RoomId = reservation.RoomId.ToString();
            entity.GuestId = reservation.GuestId.ToString();
            context.Set<ReservationEntity>().Update(entity);
        }

        public void Remove(Reservation reservation)
        {
            Check.ThrowIfNull(reservation, nameof(reservation));
            ReservationEntity entity = ReservationEntity.CreateFromModel(reservation);
            context.Set<ReservationEntity>().Remove(entity);
        }

        private ReservationEntity? FindReservationEntityById(Guid id)
        {
            return context.Set<ReservationEntity>()
                .AsNoTracking()
                .Include(e => e.Room)
                .Include(e => e.Guest)
                .SingleOrDefault(e => e.Id == id.ToString());
        }
    }
}
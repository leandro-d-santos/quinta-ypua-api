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

        public bool ExistsInPeriodByRoom(Guid roomId, DateTime checkIn, DateTime checkOut)
        {
            return context.Set<ReservationEntity>()
                .Where((reservation) => reservation.RoomId == roomId.ToString() && reservation.CheckIn <= checkOut && reservation.CheckOut >= checkIn)
                .Any();
        }

        public async Task<List<RoomStatusDto>> GetCurrentRoomsStatusAsync(List<string>? roomIds = null)
        {
            var hoje = DateTime.Today.Date;

            var query = context.Set<ReservationEntity>()
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Where(r => r.CheckIn <= hoje && r.CheckOut >= hoje);

            if (roomIds != null && roomIds.Any())
            {
                query = query.Where(r => roomIds.Contains(r.RoomId));
            }

            var reservasAtuais = await query.ToListAsync();

            // Monta o DTO
            var result = reservasAtuais.Select(r => new RoomStatusDto
            {
                RoomId = new Guid(r.RoomId),
                Descricao = r.Room.Description,
                Status = "Reservado",
                PeriodoReserva = $"{r.CheckIn:dd/MM/yyyy} - {r.CheckOut:dd/MM/yyyy}",
                Hospede = r.Guest.Name,
                RoomNumber = r.Room.Number
            }).ToList();

            // Adiciona os quartos que não têm reserva atual (status Livre)
            var reservedIds = reservasAtuais.Select(r => r.RoomId).Distinct().ToList();
            var quartosLivres = context.Set<RoomEntity>()
            .Where(room => (roomIds == null || roomIds.Contains(room.Id)) &&
               !reservedIds.Contains(room.Id))
            .Select(room => new RoomStatusDto
            {
                RoomId = new Guid(room.Id),
                Descricao = room.Description,
                Status = "Livre",
                PeriodoReserva = "Sem reserva no momento",
                Hospede = "Sem hóspede no momento",
                RoomNumber = room.Number,
            });

            result.AddRange(await quartosLivres.ToListAsync());
            return result.OrderBy(p => p.RoomNumber).ToList();
        }
    }
}
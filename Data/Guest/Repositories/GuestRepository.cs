using Data.Context;
using Data.Guests.Entities;
using Domain.Common.Utils;
using Domain.Guests.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Guests.Repositories
{
    public sealed class GuestRepository : IGuestRepository
    {
        private readonly DbConnection context;

        public GuestRepository(DbConnection context)
        {
            this.context = context;
        }

        public void Add(Guest guest)
        {
            Check.ThrowIfNull(guest, nameof(guest));
            GuestEntity entity = GuestEntity.CreateFromModel(guest);
            context.Set<GuestEntity>().Add(entity);
        }

        public IList<Guest> FindAll()
        {
            return context.Set<GuestEntity>()
                .Select((guest) => guest.TransformToModel())
                .ToList();
        }

        public Guest? FindById(Guid id)
        {
            Check.ThrowIfNull(id, nameof(id));
            GuestEntity? guest = FindGuestEntityById(id);
            if (guest is null)
            {
                return null;
            }
            return guest.TransformToModel();
        }

        public void Update(Guest guest)
        {
            Check.ThrowIfNull(guest, nameof(guest));
            GuestEntity? entity = FindGuestEntityById(guest.Id);
            if (entity is null)
            {
                return;
            }
            entity.Name = guest.Name;
            entity.Email = guest.Email;
            entity.CPF = guest.CPF;
            entity.PhoneNumber = guest.PhoneNumber;
            context.Set<GuestEntity>().Update(entity);
        }

        public void Remove(Guest guest)
        {
            Check.ThrowIfNull(guest, nameof(guest));
            GuestEntity entity = GuestEntity.CreateFromModel(guest);
            context.Set<GuestEntity>().Remove(entity);
        }

        private GuestEntity? FindGuestEntityById(Guid id)
        {
            return context.Set<GuestEntity>()
                .AsNoTracking()
                .SingleOrDefault(e => e.Id == id.ToString());
        }
    }
}
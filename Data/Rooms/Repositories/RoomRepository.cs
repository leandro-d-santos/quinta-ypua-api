using Data.Context;
using Data.Rooms.Entities;
using Domain.Common.Utils;
using Domain.Rooms.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Rooms.Repositories
{
    public sealed class RoomRepository : IRoomRepository
    {
        private readonly DbConnection context;

        public RoomRepository(DbConnection context)
        {
            this.context = context;
        }

        public void Add(Room room)
        {
            Check.ThrowIfNull(room, nameof(room));
            RoomEntity entity = RoomEntity.CreateFromModel(room);
            context.Set<RoomEntity>().Add(entity);
        }

        public IList<Room> FindAll()
        {
            return context.Set<RoomEntity>()
                .Select((room) => room.TransformToModel())
                .ToList();
        }

        public Room? FindById(Guid id)
        {
            Check.ThrowIfNull(id, nameof(id));
            RoomEntity? room = FindRoomEntityById(id);
            if (room is null)
            {
                return null;
            }
            return room.TransformToModel();
        }

        public void Update(Room room)
        {
            Check.ThrowIfNull(room, nameof(room));
            RoomEntity? entity = FindRoomEntityById(room.Id);
            if (entity is null)
            {
                return;
            }
            entity.Number = room.Number;
            entity.Floor = room.Floor;
            entity.Description = room.Description;
            entity.Capacity = room.Capacity;
            context.Set<RoomEntity>().Update(entity);
        }

        public void Remove(Room room)
        {
            Check.ThrowIfNull(room, nameof(room));
            RoomEntity entity = RoomEntity.CreateFromModel(room);
            context.Set<RoomEntity>().Remove(entity);
        }

        private RoomEntity? FindRoomEntityById(Guid id)
        {
            return context.Set<RoomEntity>()
                .AsNoTracking()
                .SingleOrDefault(e => e.Id == id.ToString());
        }
    }
}
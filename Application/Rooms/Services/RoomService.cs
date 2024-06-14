using Application.Rooms.ViewModels;
using Domain.Common.Data;
using Domain.Rooms.Models;
using Domain.Rooms.Repositories;
using System;

namespace Application.Rooms.Services
{
    public sealed class RoomService : IRoomService
    {
        private readonly IRoomRepository roomRepository;
        private readonly IUnitOfWork unitOfWork;

        public RoomService(IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            this.roomRepository = roomRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(RoomViewModel roomViewModel)
        {
            Room room = TransformToModel(roomViewModel);
            room.Id = Guid.NewGuid();
            roomRepository.Add(room);
            await unitOfWork.Complete();
            return true;
        }

        public Task<IList<RoomViewModel>> FindAllAsync()
        {
            IList<Room> rooms = roomRepository.FindAll();
            IList<RoomViewModel> roomsViewModel = rooms.Select(TransformToViewModel).ToList();
            return Task.FromResult(roomsViewModel);
        }

        public Task<RoomViewModel?> FindByIdAsync(Guid id)
        {
            Room? room = roomRepository.FindById(id);
            if (room is null)
            {
                return Task.FromResult<RoomViewModel?>(null);
            }
            return Task.FromResult<RoomViewModel?>(TransformToViewModel(room));
        }

        public async Task<bool> UpdateAsync(RoomViewModel roomViewModel)
        {
            Room? room = roomRepository.FindById(roomViewModel.Id);
            if (room is null)
            {
                throw new ArgumentException("Quarto não encontrado.");
            }
            room.Number = roomViewModel.Number;
            room.Floor = roomViewModel.Floor;
            room.Description = roomViewModel.Description;
            room.Capacity = roomViewModel.Capacity;
            roomRepository.Update(room);
            await unitOfWork.Complete();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            Room? room = roomRepository.FindById(id);
            if (room is null)
            {
                throw new ArgumentException("Quarto não encontrado.");
            }
            roomRepository.Remove(room);
            await unitOfWork.Complete();
            return true;
        }

        private static Room TransformToModel(RoomViewModel room)
        {
            return new Room()
            {
                Id = room.Id,
                Number = room.Number,
                Floor = room.Floor,
                Description = room.Description,
                Capacity = room.Capacity
            };
        }

        private static RoomViewModel TransformToViewModel(Room room)
        {
            return new RoomViewModel()
            {
                Id = room.Id,
                Number = room.Number,
                Floor = room.Floor,
                Description = room.Description,
                Capacity = room.Capacity
            };
        }
    }
}
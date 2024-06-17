using Application.Guests.ViewModels;
using Domain.Common.Data;
using Domain.Guests.Models;
using Domain.Guests.Repositories;

namespace Application.Guests.Services
{
    public sealed class GuestService : IGuestService
    {
        private readonly IGuestRepository guestRepository;
        private readonly IUnitOfWork unitOfWork;

        public GuestService(IGuestRepository guestRepository,
            IUnitOfWork unitOfWork)
        {
            this.guestRepository = guestRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(GuestViewModel guestViewModel)
        {
            Guest guest = TransformToModel(guestViewModel);
            guest.Id = Guid.NewGuid();
            guestRepository.Add(guest);
            await unitOfWork.Complete();
            return true;
        }

        public Task<IList<GuestViewModel>> FindAllAsync()
        {
            IList<Guest> guests = guestRepository.FindAll();
            IList<GuestViewModel> guestsViewModel = guests.Select(TransformToViewModel).ToList();
            return Task.FromResult(guestsViewModel);
        }

        public Task<GuestViewModel?> FindByIdAsync(Guid id)
        {
            Guest? guest = guestRepository.FindById(id);
            if (guest is null)
            {
                return Task.FromResult<GuestViewModel?>(null);
            }
            return Task.FromResult<GuestViewModel?>(TransformToViewModel(guest));
        }

        public async Task<bool> UpdateAsync(GuestViewModel guestViewModel)
        {
            Guest? guest = guestRepository.FindById(guestViewModel.Id);
            if (guest is null)
            {
                throw new ArgumentException("Hóspede não encontrado.");
            }
            guest.Name = guestViewModel.Name;
            guest.Email = guestViewModel.Email;
            guest.CPF = guestViewModel.CPF;
            guest.PhoneNumber = guestViewModel.PhoneNumber;
            guestRepository.Update(guest);
            await unitOfWork.Complete();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            Guest? guest = guestRepository.FindById(id);
            if (guest is null)
            {
                throw new ArgumentException("Hóspede não encontrado.");
            }
            guestRepository.Remove(guest);
            await unitOfWork.Complete();
            return true;
        }

        private static Guest TransformToModel(GuestViewModel guest)
        {
            return new Guest()
            {
                Id = guest.Id,
                Name = guest.Name,
                Email = guest.Email,
                CPF = guest.CPF,
                PhoneNumber = guest.PhoneNumber
            };
        }

        private static GuestViewModel TransformToViewModel(Guest guest)
        {
            return new GuestViewModel()
            {
                Id = guest.Id,
                Name = guest.Name,
                Email = guest.Email,
                CPF = guest.CPF,
                PhoneNumber = guest.PhoneNumber
            };
        }
    }
}
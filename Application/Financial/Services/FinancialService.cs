using Application.Financial.ViewModels;
using Domain.Common.Data;
using Domain.Financial.Repositories;
using Domain.Guests.Models;
using Domain.Guests.Repositories;
using Domain.Reservations.Models;
using Domain.Reservations.Repositories;
using Domain.Rooms.Models;
using Domain.Rooms.Repositories;

namespace Application.Financial.Services
{
    public sealed class FinancialService : IFinancialService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IFinancialRepository financialRepository;
        private readonly IGuestRepository guestRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IUnitOfWork unitOfWork;

        public FinancialService(
            IReservationRepository reservationRepository,
            IFinancialRepository financialRepository,
            IGuestRepository guestRepository,
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            this.reservationRepository = reservationRepository;
            this.financialRepository = financialRepository;
            this.guestRepository = guestRepository;
            this.roomRepository = roomRepository;
            this.unitOfWork = unitOfWork;
        }

        public Task<IList<FinancialViewModel>> FindAllAsync()
        {
            IList<Reservation> reservations = reservationRepository.FindAll();
            IList<Guest> guests = guestRepository.FindAll();
            IList<Room> rooms = roomRepository.FindAll();
            IList<Domain.Financial.Models.Financial> financials = financialRepository.FindAll();
            IList<FinancialViewModel> result = [];
            foreach (Reservation reservation in reservations)
            {
                Domain.Financial.Models.Financial? financial = financials.FirstOrDefault(f => f.Id == reservation.Id);
                result.Add(CreateFinancialViewModel(guests, rooms, financial, reservation));
            }
            return Task.FromResult(result);
        }

        public Task<FinancialViewModel?> FindByIdAsync(Guid id)
        {
            Reservation? reservation = reservationRepository.FindById(id);
            if (reservation is null)
            {
                return Task.FromResult<FinancialViewModel?>(null);
            }
            Domain.Financial.Models.Financial? financial = financialRepository.FindById(id);
            IList<Guest> guests = guestRepository.FindAll();
            IList<Room> rooms = roomRepository.FindAll();
            return Task.FromResult<FinancialViewModel?>(CreateFinancialViewModel(guests, rooms, financial, reservation));
        }

        public async Task<bool> UpdateAsync(FinancialViewModel financialViewModel)
        {
            Domain.Financial.Models.Financial? financial = new()
            {
                Id = financialViewModel.Id,
                Status  = financialViewModel.Status,
                ReservationValue = financialViewModel.ReservationValue,
                AdditionalValue = financialViewModel.AdditionalValue,
                Payment = financialViewModel.Payment,
            };
            financialRepository.Update(financial);
            await unitOfWork.Complete();
            return true;
        }

        public async Task<bool> CheckOutAsync(Guid id)
        {
            financialRepository.CheckOutAsync(id);
            await unitOfWork.Complete();
            return true;
        }

        private static FinancialViewModel CreateFinancialViewModel(IList<Guest> guests, IList<Room> rooms, Domain.Financial.Models.Financial? financial, Reservation reservation)
        {
            return new FinancialViewModel()
            {
                Id = reservation.Id,
                CheckIn = reservation.CheckIn,
                CheckOut = reservation.CheckOut,
                GuestId = reservation.GuestId,
                GuestName = GetGuestName(guests, reservation.GuestId),
                RoomName = GetRoomName(rooms, reservation.RoomId),
                RoomId = reservation.RoomId,
                ReservationValue = financial is not null ? financial.ReservationValue : 0,
                Payment = financial is not null ? financial.Payment : "",
                AdditionalValue = financial is not null ? financial.AdditionalValue : 0,
                Status = financial is not null ? financial.Status : "Em progresso"
            };
        }

        private static string GetGuestName(IList<Guest> guests, Guid id)
        {
            Guest? guest = guests.FirstOrDefault(g => g.Id == id);
            if (guest is null)
            {
                return "Hóspede não encontrado.";
            }
            return guest.Name;
        }

        private static string GetRoomName(IList<Room> rooms, Guid id)
        {
            Room? room = rooms.FirstOrDefault(r => r.Id == id);
            if (room is null)
            {
                return "Hóspede não encontrado.";
            }
            return room.Description;
        }

    }
}
using Application.Reservations.ViewModels;
using Domain.Common.Data;
using Domain.Financial.Repositories;
using Domain.Reservations.Models;
using Domain.Reservations.Repositories;
using Domain.Rooms.Repositories;

namespace Application.Reservations.Services
{
    public sealed class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IRoomRepository roomRepository;
        private readonly IFinancialRepository financialRepository;
        private readonly IUnitOfWork unitOfWork;

        public ReservationService(
            IReservationRepository reservationRepository,
            IFinancialRepository financialRepository,
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            this.reservationRepository = reservationRepository;
            this.roomRepository = roomRepository;
            this.financialRepository = financialRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(ReservationViewModel reservationViewModel)
        {

            if (reservationViewModel.CheckIn > reservationViewModel.CheckOut)
            {
                throw new ArgumentException("Período de reserva inválido.");
            }

            if (reservationRepository.ExistsInPeriodByRoom(reservationViewModel.RoomId, reservationViewModel.CheckIn.Date, reservationViewModel.CheckOut.Date))
            {
                throw new ArgumentException("Já existe uma reserva para esse quarto nesse período.");
            }

            Reservation reservation = TransformToModel(reservationViewModel);
            reservation.Id = Guid.NewGuid();
            reservationRepository.Add(reservation);
            financialRepository.Create(new Domain.Financial.Models.Financial() {
                Id = reservation.Id,
                ReservationValue = 0,
                AdditionalValue = 0,
                Status = "Em aberto",
                Payment = ""
            });
            await unitOfWork.Complete();
            return true;
        }
        
        public async Task<List<RoomStatusDto>> GetCurrentRoomsStatusAsync()
        {
            List<string> roomsId = roomRepository.FindAll().Select(p => p.Id.ToString()).ToList();
            return await reservationRepository.GetCurrentRoomsStatusAsync(roomsId);
        }

        public Task<IList<ReservationViewModel>> FindAllAsync()
        {
            IList<Reservation> reservations = reservationRepository.FindAll();
            IList<ReservationViewModel> reservationsViewModel = reservations.Select(TransformToViewModel).ToList();
            return Task.FromResult(reservationsViewModel);
        }

        public Task<ReservationViewModel?> FindByIdAsync(Guid id)
        {
            Reservation? reservation = reservationRepository.FindById(id);
            if (reservation is null)
            {
                return Task.FromResult<ReservationViewModel?>(null);
            }
            return Task.FromResult<ReservationViewModel?>(TransformToViewModel(reservation));
        }

        public async Task<bool> UpdateAsync(ReservationViewModel reservationViewModel)
        {
            Reservation? reservation = reservationRepository.FindById(reservationViewModel.Id);
            if (reservation is null)
            {
                throw new ArgumentException("Reserva não encontrada.");
            }
            reservation.CheckIn = reservationViewModel.CheckIn;
            reservation.CheckOut = reservationViewModel.CheckOut;
            reservation.NumberOfAdults = reservationViewModel.NumberOfAdults;
            reservation.NumberOfChildren = reservationViewModel.NumberOfChildren;
            reservation.RoomId = reservationViewModel.RoomId;
            reservation.GuestId = reservationViewModel.GuestId;
            reservationRepository.Update(reservation);
            await unitOfWork.Complete();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            Reservation? reservation = reservationRepository.FindById(id);
            if (reservation is null)
            {
                throw new ArgumentException("Reserva não encontrada.");
            }
            reservationRepository.Remove(reservation);
            await unitOfWork.Complete();
            return true;
        }

        private static Reservation TransformToModel(ReservationViewModel reservation)
        {
            return new Reservation()
            {
                Id = reservation.Id,
                CheckIn = reservation.CheckIn,
                CheckOut = reservation.CheckOut,
                NumberOfAdults = reservation.NumberOfAdults,
                NumberOfChildren = reservation.NumberOfChildren,
                RoomId = reservation.RoomId,
                GuestId = reservation.GuestId
            };
        }

        private static ReservationViewModel TransformToViewModel(Reservation reservation)
        {
            return new ReservationViewModel()
            {
                Id = reservation.Id,
                CheckIn = reservation.CheckIn,
                CheckOut = reservation.CheckOut,
                NumberOfAdults = reservation.NumberOfAdults,
                NumberOfChildren = reservation.NumberOfChildren,
                RoomId = reservation.RoomId,
                GuestId = reservation.GuestId
            };
        }
    }
}
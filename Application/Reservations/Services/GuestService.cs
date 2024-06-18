using Application.Reservations.ViewModels;
using Domain.Common.Data;
using Domain.Reservations.Models;
using Domain.Reservations.Repositories;

namespace Application.Reservations.Services
{
    public sealed class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IUnitOfWork unitOfWork;

        public ReservationService(IReservationRepository reservationRepository,
            IUnitOfWork unitOfWork)
        {
            this.reservationRepository = reservationRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(ReservationViewModel reservationViewModel)
        {
            Reservation reservation = TransformToModel(reservationViewModel);
            reservation.Id = Guid.NewGuid();
            reservationRepository.Add(reservation);
            await unitOfWork.Complete();
            return true;
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
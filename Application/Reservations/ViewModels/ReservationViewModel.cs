namespace Application.Reservations.ViewModels
{
    public sealed class ReservationViewModel
    {
        public Guid Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public Guid RoomId { get; set; }
        public Guid GuestId { get; set; }
    }
}
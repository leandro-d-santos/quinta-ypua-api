namespace Application.Financial.ViewModels
{
    public sealed class FinancialViewModel
    {
        public Guid Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public Guid GuestId { get; set; }
        public string GuestName { get; set; }
        public Guid RoomId { get; set; }
        public string RoomName { get; set; }
        public double ReservationValue { get; set; }
        public double AdditionalValue { get; set; }
        public string Payment { get; set; }
        public string Status { get; set; }
    }
}
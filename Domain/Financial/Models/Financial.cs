namespace Domain.Financial.Models
{
    public sealed class Financial
    {
        public Guid Id { get; set; }
        public double ReservationValue { get; set; }
        public double AdditionalValue { get; set; }
        public string Payment { get; set; }
        public string Status { get; set; }
    }
}
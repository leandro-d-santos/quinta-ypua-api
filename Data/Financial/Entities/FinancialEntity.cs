using Data.Common.Entities;
using Data.Guests.Entities;
using Data.Reservations.Entities;
using Data.Rooms.Entities;
using Domain.Reservations.Models;

namespace Data.Financial.Entities
{
    public class FinancialEntity : Entity
    {
        public string Id { get; set; }
        public double ReservationValue { get; set; }
        public double AdditionalValue { get; set; }
        public string Payment { get; set; }
        public string Status { get; set; }

        public static FinancialEntity CreateFromModel(Domain.Financial.Models.Financial financial)
        {
            return new FinancialEntity()
            {
                Id = financial.Id.ToString(),
                ReservationValue = financial.ReservationValue,
                AdditionalValue = financial.AdditionalValue,
                Payment = financial.Payment,
                Status = financial.Status
            };
        }

        public override Domain.Financial.Models.Financial TransformToModel()
        {
            return new()
            {
                Id = new Guid(Id),
                ReservationValue = ReservationValue,
                AdditionalValue = AdditionalValue,
                Payment = Payment,
                Status = Status
            };
        }
    }
}
using Data.Common.Entities;
using Domain.Guests.Models;

namespace Data.Guests.Entities
{
    public class GuestEntity : Entity
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public static GuestEntity CreateFromModel(Guest guest)
        {
            return new GuestEntity()
            {
                Id = guest.Id.ToString(),
                Name = guest.Name,
                Email = guest.Email,
                CPF = guest.CPF,
                PhoneNumber = guest.PhoneNumber
            };
        }

        public override Guest TransformToModel()
        {
            return new Guest()
            {
                Id = new Guid(Id),
                Name = Name,
                Email = Email,
                CPF = CPF,
                PhoneNumber = PhoneNumber
            };
        }
    }
}
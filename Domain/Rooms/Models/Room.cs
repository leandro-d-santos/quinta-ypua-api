using System;

namespace Domain.Rooms.Models
{
    public sealed class Room
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }
}
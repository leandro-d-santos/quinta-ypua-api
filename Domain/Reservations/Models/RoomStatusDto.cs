namespace Domain.Reservations.Models;

public sealed class RoomStatusDto
{
    public Guid RoomId { get; set; }
    public string RoomNumber { get; set; }
    public string Descricao { get; set; } = "";
    public string Status { get; set; } = "";
    public string? PeriodoReserva { get; set; }
    public string? Hospede { get; set; }
}
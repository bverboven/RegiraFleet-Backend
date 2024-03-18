using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Clients;

public class ClientInputDto
{
    public int Id { get; set; }
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    [MaxLength(8)]
    public string? DefaultCulture { get; set; }
}
using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Clients;

public class Client : IEntityWithSerial, IHasTitle, IHasCode
{
    public int Id { get; set; }
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(64)]
    public string Title { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}
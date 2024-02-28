using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Api.Models.Input;

public class SupplierInputDto
{
    public int? SupplierTypeId { get; set; }
    [MaxLength(16)]
    public string? Code { get; set; }
    [MaxLength(128)]
    public string? Title { get; set; }

    [MaxLength(32)]
    public string? IdentificationNumber { get; set; }
    [MaxLength(512)]
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public bool IsArchived { get; set; }
    public AddressInputDto? Address { get; set; }
    public ICollection<SupplierContactDataInputDto>? ContactData { get; set; }
}
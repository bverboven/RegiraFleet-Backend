namespace Regira.Fleet.Models.Vehicles.Brands;

public class BrandDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public string TenantId { get; set; } = null!;
    public string? Code { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }
}
namespace Regira.Fleet.Entities.Cars.Brands;

public class BrandDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public int ClientId { get; set; }
    public string? Code { get; set; }
    public string? Title { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }
}
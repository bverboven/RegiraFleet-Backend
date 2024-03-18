namespace Regira.Fleet.Entities.Clients;

public class ClientDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = System.Guid.NewGuid().ToString("N");
    public string Title { get; set; } = null!;
    public string? DefaultCulture { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }
}

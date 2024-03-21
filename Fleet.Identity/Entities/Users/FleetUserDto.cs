namespace Regira.Fleet.Identity.Entities.Users;

public class FleetUserDto
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? GivenName { get; set; }
    public string? LastName { get; set; }
    public string? Culture { get; set; }

    public ICollection<FleetClaimDto>? Claims { get; set; }
}

public class FleetClaimDto
{
    public string Type { get; set; } = null!;
    public string? Value { get; set; }
}
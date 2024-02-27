namespace Regira.Fleet.Authentication;

public class IdentityOptions
{
    public int SessionDuration { get; set; }
    public IList<FleetUser> Users { get; set; } = null!;
}
namespace Regira.Fleet.Authentication;

public class UserManager(IList<FleetUser> users)
{
    public FleetUser? Verify(string username, string password)
    {
        return users.SingleOrDefault(u =>
            u.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)
            && u.Password == password
        );
    }
}
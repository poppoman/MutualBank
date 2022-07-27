namespace MutualBank.Hubs
{
    public class UserHandler
    {
        public static HashSet<User> User = new HashSet<User>();
        public static int GetUserCount()
        {
            return User.Count;
        }
    }
    public class User
    {
        public string ConnectedId { get; set; }
        public string? Name { get; set; }
    }
}

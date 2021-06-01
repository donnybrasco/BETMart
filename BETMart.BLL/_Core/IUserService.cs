namespace BETMart.BLL._Core
{
    public interface IUserService
    {
        string Name { get; }
        int UserId { get; }
    }
    public class UserService
        : IUserService
    {
        public string Name => "BOB";
        public int UserId => 1;
    }
}

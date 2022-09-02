namespace ClientSideBoard.Models
{
    public class UserGameRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public Roles Role { get; set; }
        public User User { get; set; }
        public Game Game { get; set; }
    }
}

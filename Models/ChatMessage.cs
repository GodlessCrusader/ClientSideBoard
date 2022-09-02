

namespace ClientSideBoard.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Game Game { get; set; }
        public DateTime RecievingTime { get; set; }
        public string Text { get; set; }
    }
}



namespace ClientSideBoard.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserDisplayName { get; set; }
        public MediaType Type { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public long Weight { get; set; }
    }
    public enum MediaType
    {
        Image = 1,
        Audio = 2

    }
}

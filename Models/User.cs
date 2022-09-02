namespace ClientSideBoard.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool EmailIsConfirmed { get; set; } = false;
        public string DisplayName { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? Bio { get; set; }
        public ICollection<Game> Games { get; set; }
        public ICollection<Media> Medias { get; set; }
        public ulong MaxMediaWeight { get; set; }
        public ulong CurrentMediaWeight { get; set; } = 0;
    }
}

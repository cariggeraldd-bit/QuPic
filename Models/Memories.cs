namespace QuPic.Models
{
    public class Memories
    {
        public int Id { get; set; }
        
        public required string From { get; set; }

        public required string To { get; set; }

        public required string Message { get; set; }

        public required byte[] Img { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
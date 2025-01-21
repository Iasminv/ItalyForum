namespace ItalyForum.Models
{
    public class Comment
    {
        // primary key
        public int CommentId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Foreign key
        public int DiscussionId { get; set; }

    }
}

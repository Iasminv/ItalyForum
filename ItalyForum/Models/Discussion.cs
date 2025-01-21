namespace ItalyForum.Models
{
    public class Discussion
    {
        // primary key
        public int DiscussionId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string ImageFilename { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; } = DateTime.Now;


    }
}

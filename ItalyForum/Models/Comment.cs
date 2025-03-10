using System.ComponentModel.DataAnnotations;
using ItalyForum.Data;

namespace ItalyForum.Models
{
    public class Comment
    {
        // primary key
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Comment content is required")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Foreign key
        [Required]
        public int DiscussionId { get; set; }

        // Navigation property for Discussion - make it nullable
        public Discussion? Discussion { get; set; }

        // Foreign key (AspNetUsers table)
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        // Navigation property
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
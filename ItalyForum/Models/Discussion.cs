using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ItalyForum.Models
{
    public class Discussion
    {
        // primary key
        public int DiscussionId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [NotMapped]
        [Display(Name = "Photograph")]
        public IFormFile? ImageFile { get; set; } 

        public string? ImageFilename { get; set; } 

        public DateTime CreateDate { get; set; } = DateTime.Now;


    }
}

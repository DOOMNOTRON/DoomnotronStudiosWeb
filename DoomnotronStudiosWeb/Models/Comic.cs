using System.ComponentModel.DataAnnotations;

namespace DoomnotronStudiosWeb.Models
{
    public class Comic
    {
        [Key]
        public int Id { get; set; }

        public Creator ComicCreator { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Artist { get; set; }

        [Required]
        public string Writer { get; set; }

        public string Description { get; set; }

        [Required]
        public string rating { get; set; }

        [Required]
        public int ArcNumber { get; set; }

        [Required]
        public int IssueNumber { get; set; }

    }

    public class ComicCreateViewModel
    {
        public List<Creator>? AllAvailableComicCreators { get; set; }

        public int ChosenCreator { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Artist { get; set; }

        [Required]
        public string Writer { get; set; }

        public string Description { get; set; }

        [Required]
        public string rating { get; set; }

        [Required]
        public int ArcNumber { get; set; }

        [Required]
        public int IssueNumber { get; set; }
    }

    public class ComicIndexViewModel
    {
        public int ComicId { get; set; }

        [Display(Name = " Comic Title")]
        public string ComicTitle { get; set; }

        [Display(Name = " Creator")]
        public string ComicCreatorName { get; set; }
    }
}

﻿using System.ComponentModel.DataAnnotations;

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
}

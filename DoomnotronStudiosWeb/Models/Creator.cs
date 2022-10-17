using System.ComponentModel.DataAnnotations;

namespace DoomnotronStudiosWeb.Models
{
    public class Creator
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }
    }
}

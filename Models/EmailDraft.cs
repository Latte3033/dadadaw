using System.ComponentModel.DataAnnotations;

namespace WebStock.Models
{
    public class EmailDraft
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public string Status { get; set; } = "Send";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? SentAt { get; set; } = null;
    }
}

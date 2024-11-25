using System.ComponentModel.DataAnnotations;

namespace WebStock.Models
{
    public class EmailDraft
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The recipient's email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string To { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [StringLength(255, ErrorMessage = "Subject cannot exceed 255 characters.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Body is required.")]
        public string Body { get; set; }

        // Default status is "Unread"
        public string Status { get; set; } = "Read";

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? SentAt { get; set; } = null;

        // Method to update the status of the email draft
        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
        }
    }
}


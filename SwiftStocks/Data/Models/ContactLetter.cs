using System.ComponentModel.DataAnnotations;

namespace SwiftStocks.Data.Models
{
    public class ContactLetter
    {
        public int Id { get; set; }
        [Required]

        public string SenderName { get; set; }
        [Required]
        [EmailAddress]
        public string SenderEmail { get; set; }
        [Required]

        public string Subject { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
        public DateTime SentDate { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
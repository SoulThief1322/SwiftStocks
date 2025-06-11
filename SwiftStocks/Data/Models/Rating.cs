using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftStocks.Data.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        [ForeignKey(nameof(NewsId))]
        public News News { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public decimal Value { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
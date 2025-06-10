using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using SwiftStocks.Data.Constants;
using SwiftStocks.Data.Models;
namespace SwiftStocks.Data.Models
{
    public class News
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(NewsConstants.TitleMaxLength)]
        [MinLength(NewsConstants.TitleMinLength)]
        public string Title { get; set; }
        [Required]
        [MaxLength(NewsConstants.DescriptionMaxLength)]
        [MinLength(NewsConstants.DescriptionMinLength)]
        public string Description { get; set; }
        [Required]
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        [Required]
        [MaxLength(NewsConstants.SourceNameMaxLength)]
        [MinLength(NewsConstants.SourceNameMinLength)]
        public string SourceName { get; set; }
        public string? ImagePath { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public IdentityUser Author { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = NewsConstants.DefaultIsDeleted;
        [Required]
        public decimal Rating { get; set; } = NewsConstants.DefaultRating;

    }
}
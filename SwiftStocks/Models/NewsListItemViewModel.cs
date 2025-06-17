using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftStocks.Models
{
	public class NewsListItemViewModel
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string ImagePath { get; set; }
		public DateTime PublishedAt { get; set; }
		public string SourceName { get; set; }
		public string AuthorName { get; set; }
	}
}
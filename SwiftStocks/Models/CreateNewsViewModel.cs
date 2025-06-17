using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SwiftStocks.Models
{
	public class CreateNewsViewModel
	{
		public string Title { get; set; }
		public string Description { get; set; }

		[Display(Name = "Upload Image")]
		public IFormFile ImageFile { get; set; }
	}
}
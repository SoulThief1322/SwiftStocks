using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace SwiftStocks.Areas.Identity.Pages.Account
{
	public class ConfirmEmailModel : PageModel
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ILogger<ConfirmEmailModel> _logger;

		public ConfirmEmailModel(UserManager<IdentityUser> userManager, ILogger<ConfirmEmailModel> logger)
		{
			_userManager = userManager;
			_logger = logger;
		}

		[TempData]
		public string StatusMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(string userId, string code)
		{
			_logger.LogInformation("Starting ConfirmEmail: userId={userId}, code(raw)={code}");

			if (userId == null || code == null)
			{
				_logger.LogWarning("Missing userId or code.");
				return RedirectToPage("/Index");
			}

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				_logger.LogWarning("User not found: {userId}", userId);
				return NotFound($"Unable to load user with ID '{userId}'.");
			}

			try
			{
				code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
				_logger.LogInformation("Decoded code: {code}", code);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error decoding code.");
				StatusMessage = "Invalid code.";
				return Page();
			}

			var result = await _userManager.ConfirmEmailAsync(user, code);
			_logger.LogInformation("ConfirmEmailAsync result: {result}", result.Succeeded);

			if (result.Succeeded)
			{
				StatusMessage = "Thank you for confirming your email.";
			}
			else
			{
				foreach (var error in result.Errors)
				{
					_logger.LogError("Confirmation error: {error}", error.Description);
				}
				StatusMessage = "Error confirming your email.";
			}

			return Page();
		}

	}
}

using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

public class RegisterModel : PageModel
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly IUserStore<IdentityUser> _userStore;
	private readonly IUserEmailStore<IdentityUser> _emailStore;
	private readonly ILogger<RegisterModel> _logger;
	private readonly IEmailSender _emailSender;

	public RegisterModel(
		UserManager<IdentityUser> userManager,
		IUserStore<IdentityUser> userStore,
		SignInManager<IdentityUser> signInManager,
		ILogger<RegisterModel> logger,
		IEmailSender emailSender)
	{
		_userManager = userManager;
		_userStore = userStore;
		_emailStore = GetEmailStore();
		_signInManager = signInManager;
		_logger = logger;
		_emailSender = emailSender;
	}

	[BindProperty]
	public InputModel Input { get; set; }

	public string ReturnUrl { get; set; }

	public IList<AuthenticationScheme> ExternalLogins { get; set; }

	public class InputModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }

		[Required]
		public string Username { get; set; }
	}

	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		returnUrl ??= Url.Content("~/");

		if (ModelState.IsValid)
		{
			var user = Activator.CreateInstance<IdentityUser>();

			await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
			await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

			var result = await _userManager.CreateAsync(user, Input.Password);

			if (result.Succeeded)
			{
				_logger.LogInformation("User created a new account with password.");

				var userId = await _userManager.GetUserIdAsync(user);
				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
				var callbackUrl = Url.Page(
	"/Account/ConfirmEmail",
	pageHandler: null,
	values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
	protocol: Request.Scheme);

				await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", "Please confirm your account by clicking this link: " + callbackUrl);

				if (_userManager.Options.SignIn.RequireConfirmedAccount)
				{
					return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
				}
				else
				{
					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnUrl);
				}
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}
		return Page();
	}

	private IUserEmailStore<IdentityUser> GetEmailStore()
	{
		if (!_userManager.SupportsUserEmail)
		{
			throw new System.NotSupportedException("The default UI requires a user store with email support.");
		}
		return (IUserEmailStore<IdentityUser>)_userStore;
	}
}

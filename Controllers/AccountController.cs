using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskHive.Models;
using TaskHive.ViewModels;

public class AccountController : Controller
{
    private readonly UserManager<Users> _userManager;
    private readonly SignInManager<Users> _signInManager;

    public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Register() => View();


[HttpPost]
public async Task<IActionResult> Register(RegisterViewModel model, [FromServices] EmailSender emailSender)
{
    if (!ModelState.IsValid) return View(model);

    var user = new Users
    {
        UserName = model.Email,
        Email = model.Email,
        FullName = model.FullName
    };

    var result = await _userManager.CreateAsync(user, model.Password);

    if (result.Succeeded)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
            new { userId = user.Id, token }, Request.Scheme);

        await emailSender.SendEmailAsync(user.Email, "Confirm your email", 
            $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

        ViewBag.Message = "Registration successful. Please check your email to confirm your account.";
        return View("Info");
    }

    foreach (var error in result.Errors)
        ModelState.AddModelError("", error.Description);

    return View(model);
}

    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
            return RedirectToAction("Index", "Home");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded ? View("EmailConfirmed") : View("Error");
    
}



    public IActionResult Login() => View();

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
{
    if (!ModelState.IsValid)
        return View(model);

    var user = await _userManager.FindByEmailAsync(model.Email);

    if (user == null)
    {
        ModelState.AddModelError("", "Invalid login attempt");
        return View(model);
    }

    if (!await _userManager.IsEmailConfirmedAsync(user))
    {
        ModelState.AddModelError("", "You need to confirm your email before logging in.");
        return View(model);
    }

    var result = await _signInManager.PasswordSignInAsync(
        user, model.Password, model.RememberMe, lockoutOnFailure: true);

    if (result.Succeeded)
    {

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    if (result.IsLockedOut)
    {
        ModelState.AddModelError("", "Your account is temporarily locked due to multiple failed login attempts. Please try again later.");
        return View(model);
    }

    if (result.IsNotAllowed)
    {
        ModelState.AddModelError("", "You are not allowed to login. Please confirm your email first.");
        return View(model);
    }

    ModelState.AddModelError("", "Invalid login attempt");
    return View(model);
}



    public IActionResult ForgotPassword() => View();

[HttpPost]
public async Task<IActionResult> ForgotPassword(string email, [FromServices] EmailSender emailSender)
{
    if (string.IsNullOrEmpty(email)) return View();

    var user = await _userManager.FindByEmailAsync(email);
    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        return View("ForgotPasswordConfirmation"); 

    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    var resetLink = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

    await emailSender.SendEmailAsync(user.Email, "Reset Password",
        $"Reset your password by <a href='{resetLink}'>clicking here</a>.");

    return View("ForgotPasswordConfirmation");
}

public IActionResult ResetPassword(string token, string email)
{
    if (token == null || email == null) return RedirectToAction("Index", "Home");
    var model = new ResetPasswordViewModel { Token = token, Email = email };
    return View(model);
}

[HttpPost]
public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
{
    if (!ModelState.IsValid) return View(model);

    var user = await _userManager.FindByEmailAsync(model.Email);
    if (user == null) 
        return RedirectToAction(nameof(ResetPasswordConfirmation));

    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
    if (result.Succeeded) 
        return RedirectToAction(nameof(ResetPasswordConfirmation));

    foreach (var error in result.Errors) 
        ModelState.AddModelError("", error.Description);
    
    return View(model);
}

public IActionResult ResetPasswordConfirmation()
{
    return View();
}




    public IActionResult ResendEmailConfirmation() => View();

[HttpPost]
public async Task<IActionResult> ResendEmailConfirmation(string email, [FromServices] EmailSender emailSender)
{
    if (string.IsNullOrEmpty(email)) return View();

    var user = await _userManager.FindByEmailAsync(email);
    if (user == null || await _userManager.IsEmailConfirmedAsync(user))
        return View("ResendEmailConfirmationSent");

    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme);

    await emailSender.SendEmailAsync(user.Email, "Confirm your email", 
        $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

    return View("ResendEmailConfirmationSent");
}



    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}

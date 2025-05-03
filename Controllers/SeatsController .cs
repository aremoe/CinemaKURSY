using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;

public class SeatsController : Controller
{
    private readonly IEmailSender _emailSender;

    public SeatsController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    [HttpGet]
    public async Task<IActionResult> TestEmail()
    {
        try
        {
            string testEmail = "testuser@example.com";
            string subject = "Test Email from Cinema App";
            string message = "<h1>This is a test email</h1><p>If you received this email, the email sending functionality works correctly.</p>";

            await _emailSender.SendEmailAsync(testEmail, subject, message);

            return Content("Test email sent successfully!");
        }
        catch (Exception ex)
        {
            return Content($"Failed to send test email: {ex.Message}");
        }
    }
}

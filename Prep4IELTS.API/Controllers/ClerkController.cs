using System.Net;
using EXE202_Prep4IELTS.Payloads.Requests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Svix;

namespace EXE202_Prep4IELTS.Controllers
{
    [ApiController]
    [Route("/api/webhook/clerk")]
    public class ClerkController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ClerkController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            var clerkWebhookSecret = _configuration["Clerk:WebHookSecret"]!;

            if (string.IsNullOrEmpty(clerkWebhookSecret))
            {
                return StatusCode(500, "Please add WEBHOOK_SECRET from Clerk Dashboard to the configuration.");
            }

            var svixId = Request.Headers["svix-id"];
            var svixTimestamp = Request.Headers["svix-timestamp"];
            var svixSignature = Request.Headers["svix-signature"];

            if (string.IsNullOrEmpty(svixId) || string.IsNullOrEmpty(svixTimestamp) ||
                string.IsNullOrEmpty(svixSignature))
            {
                return BadRequest("Missing Svix headers.");
            }

            var headers = new WebHeaderCollection();
            headers.Set("svix-id", svixId);
            headers.Set("svix-timestamp", svixTimestamp);
            headers.Set("svix-signature", svixSignature);

            string payload;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                payload = await reader.ReadToEndAsync();
            }

            var wh = new Webhook(clerkWebhookSecret);

            try
            {
                wh.Verify(payload, headers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Svix error: {ex.Message}");
            }

            var webhookPayload = JsonConvert.DeserializeObject<WebhookPayload>(payload);

            if (webhookPayload is null)
            {
                return BadRequest($"Missing payload");
            }

            if (webhookPayload.Type == "user.created")
            {
                var email = webhookPayload.Data.EmailAddresses[0].EmailAddress;
                var username = webhookPayload.Data.Username;
                var clerkId = webhookPayload.Data.Id;
                var avatar = webhookPayload.Data.ImageUrl;

                //create user here
                return Ok(new { success = true, message = "User created", clerkId, email });
            }

            if (webhookPayload.Type == "user.updated")
            {
                var email = webhookPayload.Data.EmailAddresses[0].EmailAddress;
                var username = webhookPayload.Data.Username;
                var clerkId = webhookPayload.Data.Id;
                var avatar = webhookPayload.Data.ImageUrl;

                //update user here, find user by clerkId and update

                return Ok(new { success = true, message = "User updated", clerkId, email });
            }

            return Ok(new { success = true, message = "Webhook received" });
        }
    }
}
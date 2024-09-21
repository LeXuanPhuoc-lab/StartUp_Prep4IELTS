using System.Net;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Requests;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Svix;
using SystemRole = Prep4IELTS.Data.Enum.SystemRole;

namespace EXE202_Prep4IELTS.Controllers
{
    [ApiController]
    [Route(ApiRoute.Clerk.UserSyncWebhook)]
    public class ClerkController(
        IConfiguration configuration,
        ISystemRoleService roleService,
        IUserService userService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            var clerkWebhookSecret = configuration["Clerk:WebHookSecret"] ?? string.Empty;

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
                return BadRequest("Missing payload");
            }

            if (webhookPayload.Type == "user.created")
            {
                var email = webhookPayload.Data.EmailAddresses[0].EmailAddress;
                var username = webhookPayload.Data.Username;
                var clerkId = webhookPayload.Data.Id;
                var avatar = webhookPayload.Data.ImageUrl;

                if (string.IsNullOrEmpty(clerkId)) return BadRequest("Missing clerk ID");
                
                // Mark whether user created or updated 
                bool isRowEffected = false;
                // Check exist user by email 
                var userDto = await userService.FindByEmailAsync(email);
                if (userDto != null) // User basic information already initiated 
                {
                    // Initiate new userDto to update already exist user information
                    var toUpdateUser = new UserDto(
                        userDto.Id, userDto.UserId, clerkId,
                        FirstName: userDto.FirstName, LastName: userDto.LastName,
                        Email: email, Username: username, Phone: userDto.Phone,
                        AvatarImage: avatar, IsActive: userDto.IsActive,
                        DateOfBirth: userDto.DateOfBirth, CreateDate: userDto.CreateDate,
                        TestTakenDate: userDto.TestTakenDate, TargetScore: userDto.TargetScore, 
                        RoleId: userDto.RoleId, Role: userDto.Role);
                    // Progress update user information
                    isRowEffected = await userService.UpdateAsync(
                        toUpdateUser, isUpdateClerkId: true, isUpdateRole: false);
                }
                else
                {
                    // Get student role 
                    var studentRole = await roleService.FindByRoleNameAsync(SystemRole.Student);
                    // Create new user 
                    var toInsertUser = new UserDto(
                        Id:0, UserId: Guid.Empty, clerkId,
                        FirstName: string.Empty, LastName: string.Empty,
                        Email: email, Username: username, Phone: null,
                        AvatarImage: avatar, IsActive: null,
                        DateOfBirth: null, CreateDate: DateTime.UtcNow,
                        TestTakenDate: null, TargetScore: null, 
                        // Default is [Student] role
                        RoleId: studentRole.RoleId,
                        Role: null!);
                    // Progress create new user 
                    isRowEffected = await userService.InsertAsync(toInsertUser);
                }
                
                if(isRowEffected)
                    //create user here
                    return Ok(new { success = true, message = "User created", clerkId, email });
                
                // Invoke error while create or update user 
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }

            if (webhookPayload.Type == "user.updated")
            {
                var email = webhookPayload.Data.EmailAddresses[0].EmailAddress;
                var username = webhookPayload.Data.Username;
                var clerkId = webhookPayload.Data.Id;
                var avatar = webhookPayload.Data.ImageUrl;
                
                if (string.IsNullOrEmpty(clerkId)) return BadRequest("Missing clerk ID");
                
                // Check exist user by email 
                var userDto = await userService.FindByEmailAsync(email);
                if (userDto != null) // User basic information already initiated 
                {
                    // Initiate new userDto to update already exist user information
                    var toUpdateUser = new UserDto(
                        userDto.Id, userDto.UserId, clerkId,
                        FirstName: userDto.FirstName, LastName: userDto.LastName,
                        Email: email, Username: username, Phone: userDto.Phone,
                        AvatarImage: avatar, IsActive: userDto.IsActive,
                        DateOfBirth: userDto.DateOfBirth, CreateDate: userDto.CreateDate,
                        TestTakenDate: userDto.TestTakenDate, TargetScore: userDto.TargetScore, 
                        RoleId: userDto.RoleId, Role: userDto.Role);
                    // Progress update user information
                    var isUpdated = await userService.UpdateAsync(
                        toUpdateUser, isUpdateClerkId: false, isUpdateRole: false);
                    
                    return isUpdated // Is update successfully
                        ? Ok(new { success = true, message = "User updated", clerkId, email })
                        : StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
                }
                
                // Not found any user match
                return NotFound($"Not found any user match email: {email}");
            }

            return Ok(new { success = true, message = "Webhook received" });
        }
    }
}
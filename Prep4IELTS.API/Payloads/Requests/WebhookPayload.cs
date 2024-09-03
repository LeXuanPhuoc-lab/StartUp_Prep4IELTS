using Newtonsoft.Json;

namespace EXE202_Prep4IELTS.Payloads.Requests;

public class WebhookPayload
{
    public string Type { get; set; } = null!; //"user.created","user.updated","user.deleted"
    public WebhookData Data { get; set; } = null!;
}

public class WebhookData
{
    public string Id { get; set; } = null!; //clerkId

    public string Username { get; set; } = null!;

    [JsonProperty("first_name")] public string FirstName { get; set; } = null!;

    [JsonProperty("last_name")] public string LastName { get; set; } = null!;

    [JsonProperty("image_url")] public string ImageUrl { get; set; } = null!;

    [JsonProperty("email_addresses")] public List<EmailAddressObject> EmailAddresses { get; set; } = null!;
}

public class EmailAddressObject
{
    [JsonProperty("email_address")] public string EmailAddress { get; set; } = null!;
}
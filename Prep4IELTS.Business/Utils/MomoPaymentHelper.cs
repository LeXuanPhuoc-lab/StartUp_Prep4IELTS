namespace Prep4IELTS.Business.Utils;

public class MomoPaymentHelper
{
    public static string GenerateRequestId()
    {
        return $"RE{GenerateRandomDigits(11)}";
    }

    public static string GenerateOrderId(string requestId)
    {
        if (!string.IsNullOrEmpty(requestId) 
            && requestId.Length > 2 
            && requestId.StartsWith("RE"))
        {
            string uniqueDigits = requestId.Substring(2);
            return $"OD{uniqueDigits}";
        }

        return Guid.NewGuid().ToString();
    }
    
    // Generate random digits
    public static string GenerateRandomDigits(int length)
    {
        var rnd = new Random();
        string digits = string.Empty;
        
        for(int i = 0; i < length; ++i)
        {
            digits += rnd.Next(0, 10); // Random each digit from 0 and 9
        }

        return digits;
    }
}
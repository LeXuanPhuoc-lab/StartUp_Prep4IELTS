namespace Prep4IELTS.Business.Utils;

public class PaymentHelper
{
    public static string GenerateRequestId()
    {
        return $"RE{GenerateRandomDigits(11)}";
    }

    public static int GenerateRandomOrderCodeDigits(int length)
    {
        return int.Parse(GenerateRandomDigitsWithTimeStamp(length));
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
    
    private static string GenerateRandomDigitsWithTimeStamp(int length)
    {
        var rnd = new Random();
    
        // Get a timestamp (ticks)
        long timestamp = DateTime.Now.Ticks;
    
        // Use the last part of the timestamp to ensure limited size 
        string timestampPart = timestamp.ToString().Substring(timestamp.ToString().Length - Math.Min(8, length));

        // Generate the random digits portion
        string digits = string.Empty;
        for (int i = 0; i < length - timestampPart.Length; ++i)
        {
            digits += rnd.Next(0, 10); 
        }

        // Combine random digits with timestamp part
        return digits + timestampPart;
    }
}
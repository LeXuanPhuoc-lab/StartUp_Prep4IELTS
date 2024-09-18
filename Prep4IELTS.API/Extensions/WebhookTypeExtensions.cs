using System.Collections;
using EXE202_Prep4IELTS.Payloads.Responses.Payments.PayOS;
using Prep4IELTS.Business.Utils;

namespace EXE202_Prep4IELTS.Extensions;

public static class WebhookTypeExtensions
{
    public static async Task<string> GenerateWebhookSignatureAsync(this PayOSPaymentLinkInformationResponse resp, string paymentLinkId, string checksumKey)
    {
        var rawSignature = $"orderCode={resp.Data.OrderCode}&amount={resp.Data.Amount}&description={resp.Data.Transactions[0].Description}" +
                            $"&accountNumber={resp.Data.Transactions[0].AccountNumber}&reference={resp.Data.Transactions[0].Reference}&transactionDateTime={resp.Data.Transactions[0].TransactionDateTime}" +
                            $"&currency=VND&paymentLinkId={paymentLinkId}&code={resp.Code}&desc={resp.Desc}" +
                            $"&counterAccountBankId={resp.Data.Transactions[0].CounterAccountBankId}&counterAccountBankName={resp.Data.Transactions[0].CounterAccountBankName}" +
                            $"&counterAccountName={resp.Data.Transactions[0].CounterAccountName}&counterAccountNumber={resp.Data.Transactions[0].CounterAccountNumber}" +
                            $"&virtualAccountName={resp.Data.Transactions[0].VirtualAccountName}&virtualAccountNumber={resp.Data.Transactions[0].VirtualAccountNumber}";
        // Split the raw signature string into key-value pairs
        List<string> keyValuePairs = rawSignature.Split('&').ToList();

        // Sort the key-value pairs based on the key
        keyValuePairs.Sort((pair1, pair2) =>
        {
            var key1 = pair1.Split('=')[0];
            var key2 = pair2.Split('=')[0];
            return string.Compare(key1, key2, StringComparison.Ordinal);
        });

        // Join the sorted key-value pairs back into a single string
        string sortedRawSignature = string.Join("&", keyValuePairs);

        // Generate the HMAC hash using the sorted string
        return await Task.FromResult(HashHelper.HmacSha256(sortedRawSignature, checksumKey));
    }
}
namespace Prep4IELTS.Business.Constants;

public static class MomoResultCodeConstants
{
    public const string Successful = "0";
    public const string AccessDenied = "11";
    public const string BadFormatRequest = "20";
    public const string DuplicatedRequestId = "40";
    public const string DuplicatedOrderId = "41";
    public const string WaitingForConfirm = "1000";
    public const string InSufficientFunds = "1001";
    public const string RejectedByIssuers = "1002";
    public const string CancelledByUser = "1003";
    public const string OverTransactionTimeLimit = "1004";
    public const string QrCodeExpired = "1005";
    public const string UserDeniedConfirmPayment = "1006";
    public const string TransactionIsAuthorizedSuccessfully = "9000";
}
namespace EXE202_Prep4IELTS.Payloads;

public static class ApiRoute
{
    private const string Base = "api";

    public static class Clerk
    {
        public const string UserSyncWebhook = Base + "/webhook/clerk";
    }
    
    public static class User
    {
        public const string WhoAmI = Base + "/users/who-am-i";
        public const string Create = Base + "/users/create";
        public const string Update = Base + "/users/{userId}/update";
        public const string Delete = Base + "/users/{userId}/delete";
        public const string GetAllUserRole = Base + "/user-roles";
    }

    public static class Test
    {
        // [HTTP GET]
        public const string GetAll = Base + "/tests";
        public const string GetAllDraft = Base + "/tests/draft";
        public const string GetById = Base + "/tests/{id}";
        public const string GetAllAnswer = Base + "/tests/{id}/answers";
        public const string GetCreateTestDetail = Base + "/tests/create/detail";
        public const string PracticeById = Base + "/tests/{id}/practice";
        public const string StartTest = Base + "/tests/{id}/start";
        public const string GetForReSubmit = Base + "/tests/{id}/re-submit";
        public const string GetAnalyticsTimeFilter = Base + "/tests/analytics/time-filter";
        public const string GetAnalytics = Base + "/tests/analytics";
        
        // [HTTP POST]
        public const string Submission = Base + "/tests/submission";
        public const string ReSubmit = Base + "/tests/{id}/re-submit";
        public const string Create = Base + "/tests/create";
        
        // [HTTP PUT]
        public const string Update = Base + "/tests/{id}/update";
        
        // [HTTP PATCH]
        public const string Publish = Base + "/tests/{id}/publish";
        public const string Hidden = Base + "/tests/{id}/hidden";
        
        // [HTTP DELETE]
        public const string Delete = Base + "/tests/{id}/delete";
    }

    public static class TestCategory
    {
        public const string GetAll = Base + "/test-categories";
    }

    public static class TestHistory
    {
        public const string GetAllByUserId = Base + "/test-histories";
        public const string GetHistoryById = Base + "/test-histories/{id}";

        public const string GetPartitionHistoryWithGradeById =
            Base + "/test-histories/partitions/{partitionId}/test-grades/{testGradeId}";
    }

    public static class Comment
    {
        public const string GetAllByTestId = Base + "/comments/{testId}";
    }

    public static class Payment
    {
        public const string CreatePaymentWithMethod = Base + "/payment/create";
        public const string GetPaymentIssuers = Base + "/payment/issuers";
        public const string GetAllPaymentType = Base + "/payment/types";
        
        // MOMO
        public const string GetMomoPaymentMethods = Base + "/payment/momo-methods";
        public const string CheckTransactionStatus = Base + "/payment/momo/transaction-status/{orderId}";
        public const string Confirm = Base + "/payment/momo/confirm";
        public const string InitiateTransaction = Base + "/payment/momo/initiate-transaction";
        public const string MomoReturn = Base + "/payment/momo-return";
        public const string MomoIpn = Base + "/payment/momo-ipn";
        
        // PayOS
        public const string GetPayOsPaymentLinkInformation = Base + "/payment/pay-os/payment-link-information/{paymentLinkId}";
        public const string CancelPayOsPayment = Base + "/payment/pay-os/cancel-payment/{paymentLinkId}";
        public const string VerifyPaymentWebhookData = "/payment/pay-os/verify-payment-webhook-data";
        public const string WebhookPayOsReturn = Base + "/payment/pay-os/return";
        public const string WebhookPayOsCancel = Base + "/payment/pay-os/cancel";
    }

    public static class Resource
    {
        public const string GetFileType = Base + "/resources/file-type";
        public const string UploadImage = Base + "/resources/image/upload";
        public const string UploadAudio = Base + "/resources/audio/upload";
        public const string UpdateImage = Base + "/resources/image/update";
        public const string UpdateAudio = Base + "/resources/audio/update";
        public const string Delete = Base + "/resources/delete";
    }

    public static class PremiumPackage
    {
        public const string GetAll = Base + "/premium-packages";
        public const string GetAllDraft = Base + "/premium-packages/draft";
        public const string Create = Base + "/premium-packages/create";
        public const string Publish = Base + "/premium-packages/{premiumPackageId}/publish";
        public const string Hidden = Base + "/premium-packages/{premiumPackageId}/hidden";
        public const string Update = Base + "/premium-packages/{premiumPackageId}/update";
        public const string Delete = Base + "/premium-packages/{premiumPackageId}/delete";
    }
}
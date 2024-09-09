namespace EXE202_Prep4IELTS.Payloads;

public static class ApiRoute
{
    private const string Base = "api";

    public static class User
    {
        public const string WhoAmI = Base + "/users/who-am-i";
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
        
        // [HTTP POST]
        public const string Submission = Base + "/tests/submission";
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
        
        // MOMO
        public const string GetMomoPaymentMethods = Base + "/payment/momo-methods";
        public const string CheckTransactionStatus = Base + "/payment/momo/transaction-status/{orderId}";
        public const string Confirm = Base + "/payment/momo/confirm";
        public const string InitiateTransaction = Base + "/payment/momo/initiate-transaction";
        public const string MomoReturn = Base + "/payment/momo-return";
        public const string MomoIpn = Base + "/payment/momo-ipn";
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
}
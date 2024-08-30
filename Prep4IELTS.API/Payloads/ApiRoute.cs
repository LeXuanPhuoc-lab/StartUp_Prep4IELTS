namespace EXE202_Prep4IELTS.Payloads;

public static class ApiRoute
{
    private const string Base = "api";

    public static class Test
    {
        public const string GetAll = Base + "/tests";
        public const string GetById = Base + "/tests/{id}";
        public const string PracticeById = Base + "/tests/{id}/practice";
        public const string StartTest = Base + "/tests/{id}/start";
        public const string GetAllAnswer = Base + "/tests/{id}/answers";
        public const string Submission = Base + "/tests/submission";
    }

    public static class TestCategory
    {
        public const string GetAll = Base + "/test-categories";
    }

    public static class TestHistory
    {
        public const string GetAllByUserId = Base + "/test-histories";
        public const string GetHistoryById = Base + "/test-histories/{id}";
        public const string GetPartitionHistoryWithGradeById = Base + "/test-histories/partitions/{partitionId}/test-grades/{testGradeId}";
    }

    public static class Comment
    {
        public const string GetAllByTestId = Base + "/comments/{testId}";
    }

    public static class Payment
    {
        public const string CreatePaymentWithMethod = Base + "/payment/create";
        public const string MomoReturn = Base + "/payment/momo-return";
        public const string MomoIpn = Base + "/payment/momo-ipn";
    }
}
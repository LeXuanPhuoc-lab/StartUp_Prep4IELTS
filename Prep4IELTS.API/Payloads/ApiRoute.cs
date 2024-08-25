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
    }

    public static class TestCategory
    {
        public const string GetAll = Base + "/test-categories";
    }

    public static class Comment
    {
        public const string GetAllByTestId = Base + "/comments/{testId}";
    }
}
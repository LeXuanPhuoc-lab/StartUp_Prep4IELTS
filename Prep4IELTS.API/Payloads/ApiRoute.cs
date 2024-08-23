namespace EXE202_Prep4IELTS.Payloads;

public static class ApiRoute
{
    private const string Base = "api";

    public static class Test
    {
        public const string GetAll = Base + "/tests";
        public const string GetById = Base + "/tests/{id}";
        public const string GetByCategory = Base + "/tests/{category}";
    }

    public static class TestCategory
    {
        public const string GetAll = Base + "/test-categories";
    }
}
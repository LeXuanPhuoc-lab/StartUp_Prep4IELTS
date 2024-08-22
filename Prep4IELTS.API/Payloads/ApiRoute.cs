namespace EXE202_Prep4IELTS.Payloads;

public static class ApiRoute
{
    private const string Base = "api";

    public static class Test
    {
        public const string GetAll = Base + "/Tests";
        public const string GetById = Base + "/Tests/{testId:Guid}";
    }
}
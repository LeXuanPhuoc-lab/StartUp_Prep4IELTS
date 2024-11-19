namespace EXE202_Prep4IELTS.Payloads.Filters
{
	public class TransactionFilterRequest
	{
        public string? Sort { get; set; } = string.Empty;
        public string? SearchValue { get; set; } = string.Empty;
        public int PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } 
    }
}

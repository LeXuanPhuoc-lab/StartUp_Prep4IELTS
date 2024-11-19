using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Filters;
using EXE202_Prep4IELTS.Payloads.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Controllers
{
	[ApiController]
	public class TransactionController(
		ITransactionService transactionService,
		IOptionsMonitor<AppSettings> monitor) : ControllerBase
	{
		private readonly AppSettings _appSettings = monitor.CurrentValue;

		[HttpGet(ApiRoute.Transaction.GetAll, Name = nameof(GetAllTransactionAsync))]
		[ClerkAuthorize(Roles = [SystemRoleConstants.Admin])]
		public async Task<IActionResult> GetAllTransactionAsync([FromQuery] TransactionFilterRequest req)
		{
			// Get all transactions 
			var transactions = await transactionService.FindAllAsync();

			// Progress search (if any)
			if (!string.IsNullOrEmpty(req.SearchValue))
			{
				// Search text
				transactions = transactions.Where(t =>
					t.TransactionStatus.Contains(req.SearchValue) || 
					t.TransactionCode.Contains(req.SearchValue) || 
					t.PaymentAmount.ToString().Contains(req.SearchValue)).ToList();
			}

			// Progress sorting
			if (!string.IsNullOrEmpty(req.Sort))
			{
				transactions = (await SortHelper.SortTransactionByColumnAsync(
					transactions, req.Sort)).ToList();
			}
			
			// Progress paging
			var pagingList = PaginatedList<TransactionDto>.Paginate(transactions, req.PageIndex, 
				req.PageSize ?? _appSettings.PageSize);

			return !pagingList.Any() // Not exist any transaction
			   ? NotFound(new BaseResponse()
			   {
				   StatusCode = StatusCodes.Status404NotFound,
				   Message = "Not found any transactions."
			   })
			   : Ok(new BaseResponse()
			   {
				   StatusCode = StatusCodes.Status200OK,
				   Data = new
				   {
					   Users = pagingList,
					   Page = pagingList.PageIndex,
					   TotalPage = pagingList.TotalPage
				   }
			   });
		}
	}
}

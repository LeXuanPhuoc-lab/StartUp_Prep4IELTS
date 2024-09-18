using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IPaymentTypeService
{
    Task<IList<PaymentTypeDto>> FindAllAsync();
    Task<PaymentTypeDto?> FindByIdAsync(int paymentTypeId);
}
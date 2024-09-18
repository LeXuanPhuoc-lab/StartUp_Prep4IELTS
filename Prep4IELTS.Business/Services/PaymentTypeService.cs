using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services;

public class PaymentTypeService(UnitOfWork unitOfWork) : IPaymentTypeService
{
    public async Task<IList<PaymentTypeDto>> FindAllAsync()
    {
        var paymentTypeEntities = await unitOfWork.PaymentTypeRepository.FindAllAsync();
        return paymentTypeEntities.Adapt<List<PaymentTypeDto>>();
    }

    public async Task<PaymentTypeDto?> FindByIdAsync(int paymentTypeId)
    {
        var paymentTypeEntity = await unitOfWork.PaymentTypeRepository.FindAsync(paymentTypeId);
        return paymentTypeEntity.Adapt<PaymentTypeDto>();
    }
}
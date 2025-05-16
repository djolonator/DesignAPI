
using Infrastracture.Abstractions;
using Infrastracture.Models;
using Infrastructure.Abstractions;

namespace Infrastracture.Interfaces.IServices.External
{
    public interface IPrintfullService
    {
        Task<Result<EstimatePrintfullOrderCosts>> EstimatePrintfullOrderCosts(CheckoutRequest checkoutRequest, CreatePrintfullOrderRequest orderBody);
        Task<Result<PrintfullOrderResponse>> CreatePrintfullOrder(Domain.Entities.Order userOrder, CreatePrintfullOrderRequest orderBody);
        void CancelPrintfullOrder(long orderId);
        Task<Result<PrintfullOrderResponseGet>> GetPrintfullOrder(long orderId);
        Task<Result<Generic>> ConfirmPrintfullOrder(long orderId);
    }
}

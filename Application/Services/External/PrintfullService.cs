using Infrastracture.Interfaces.IServices.External;
using Infrastracture.Models;
using Infrastructure.Abstractions;
using System.Net.Http.Json;
using System.Text.Json;
using Infrastructure.Abstractions.Errors;
using Infrastracture.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Services.External
{
    public class PrintfullService : IPrintfullService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        public PrintfullService(IHttpClientFactory httpClientFactory, ILogger<PrintfullService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Result<EstimatePrintfullOrderCosts>> EstimatePrintfullOrderCosts(CheckoutRequest checkoutRequest, CreatePrintfullOrderRequest orderBody)
        {
            var client = _httpClientFactory.CreateClient("printfull");
            var result = new HttpResponseMessage();

            try
            {
                result = await client.PostAsJsonAsync<CreatePrintfullOrderRequest>("/orders/estimate-costs", orderBody);
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    return Result<EstimatePrintfullOrderCosts>.Success(JsonSerializer.Deserialize<EstimatePrintfullOrderCosts>(content));
                }
                else
                {
                    var error = JsonSerializer.Deserialize<ErrorResponsePrintfull>(content);
                    _logger.LogError(error.Error.Message, "Error in: PrintfullService/orders/estimate-costs");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: PrintfullService/orders/estimate-costs");
            }

            return Result<EstimatePrintfullOrderCosts>.Failure(new Error("Could not process order right now"));
        }

        public async Task<Result<PrintfullOrderResponse>> CreatePrintfullOrder(Domain.Entities.Order userOrder, CreatePrintfullOrderRequest orderBody)
        {
            var client = _httpClientFactory.CreateClient("printfull");
            var result = new HttpResponseMessage();

            try
            {
                result = await client.PostAsJsonAsync<CreatePrintfullOrderRequest>("/orders?confirm=true", orderBody);
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    return Result<PrintfullOrderResponse>.Success(JsonSerializer.Deserialize<PrintfullOrderResponse>(content));
                }
                else
                {
                    var error = JsonSerializer.Deserialize<ErrorResponsePrintfull>(content);
                    _logger.LogError(error.Error.Message, "Error in: PrintfullService.CreatePrintfullOrder()");
                }

            }
            catch (Exception ex)
            {
                //error message from printfull api resposnse for logs
                _logger.LogError(ex, "Error in: PrintfullService.CreatePrintfullOrder()");
            }

            return Result<PrintfullOrderResponse>.Failure(new Error("Could not process order right now"));
        }

        public void CancelPrintfullOrder(long orderId)
        {
            var client = _httpClientFactory.CreateClient("printfull");
            Task.Run(async () =>
            {
                try
                {
                    var result = await client.DeleteAsync($"/orders/{orderId}");
                }
                catch (Exception ex)
                {

                }
            });
        }

        public async Task<Result<PrintfullOrderResponseGet>> GetPrintfullOrder(long orderId)
        {
            var client = _httpClientFactory.CreateClient("printfull");

            var result = new HttpResponseMessage();

            try
            {
                result = await client.GetAsync($"/orders/{orderId}");
                var content = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    
                    return Result<PrintfullOrderResponseGet>.Success(JsonSerializer.Deserialize<PrintfullOrderResponseGet>(content));
                }
                else
                {
                    var error = JsonSerializer.Deserialize<ErrorResponsePrintfull>(content);
                    _logger.LogError(error.Error.Message, "Error in: PrintfullService.GetPrintfullOrder()");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: PrintfullService.GetPrintfullOrder()");
            }

            return Result<PrintfullOrderResponseGet>.Failure(new Error("Message from response"));//error message from printfull api resposnse
        }

        public async Task<Result<Generic>> ConfirmPrintfullOrder(long orderId)
        {
            var client = _httpClientFactory.CreateClient("printfull");

            var result = new HttpResponseMessage();

            try
            {
                result = await client.PostAsync($"/orders/{orderId}/confirm", null);

                if (result.IsSuccessStatusCode)
                {

                    return Result<Generic>.Success(new Generic() { Value = "Order confirmed" });
                }
                else
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var error = JsonSerializer.Deserialize<ErrorResponsePrintfull>(content);
                    _logger.LogError(error.Error.Message, "Error in: PrintfullService.GetPrintfullOrder()");
                    return Result<Generic>.Failure(new Error("Could not confirm order"));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in: PrintfullService.GetPrintfullOrder()");
            }

            return Result<Generic>.Failure(new Error("Could not confirm order"));
        }
    }
}

using Infrastracture.Interfaces.IServices.External;
using Infrastracture.Models;
using Infrastructure.Abstractions;
using System.Net.Http.Json;
using System.Text.Json;
using Infrastructure.Abstractions.Errors;
using Infrastracture.Abstractions;

namespace Application.Services.External
{
    public class PrintfullService : IPrintfullService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public PrintfullService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
                }

            }
            catch (Exception ex)
            {
                //error message from printfull api resposnse for logs
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
                }

            }
            catch (Exception ex)
            {
                //error message from printfull api resposnse for logs
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

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    return Result<PrintfullOrderResponseGet>.Success(JsonSerializer.Deserialize<PrintfullOrderResponseGet>(content));
                }

            }
            catch (Exception ex)
            {
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
                    return Result<Generic>.Failure(new Error("Could not confirm order"));
                }

            }
            catch (Exception ex)
            {
            }

            return Result<Generic>.Failure(new Error("Could not confirm order"));
        }
    }
}

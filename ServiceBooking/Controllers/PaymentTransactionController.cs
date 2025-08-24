using ApiDay1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBooking.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace ServiceBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTransactionController : ControllerBase
    {
        private readonly string _clientId;
        private readonly string _secret;
        private readonly string _url;
        private readonly MyContext _context;
        public PaymentTransactionController(IConfiguration config, MyContext context)
        {
            _clientId = config["PayPalSetting:ClientId"]!;
            _secret = config["PayPalSetting:Secret"]!;
            _url = config["PayPalSetting:Url"]!;
            _context = context;
        }



        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] JsonObject data)
        {
            string? totalAmount = data?["amount"]?.ToString();

            if (string.IsNullOrWhiteSpace(totalAmount))
                return BadRequest(new { error = "Amount is required" });

            // إنشاء JSON للطلب كما يتطلبه PayPal
            var createOrderRequest = new JsonObject
            {
                ["intent"] = "CAPTURE",
                ["purchase_units"] = new JsonArray
        {
            new JsonObject
            {
                ["amount"] = new JsonObject
                {
                    ["currency_code"] = "USD",
                    ["value"] = totalAmount
                }
            }
        }
            };

            // جلب Access Token
            string accessToken = await GetPaypalAccessTokenInternal();
            if (string.IsNullOrEmpty(accessToken))
                return StatusCode(500, new { error = "Failed to retrieve access token" });

            string url = $"{_url}/v2/checkout/orders";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var content = new StringContent(createOrderRequest.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

            var strResponse = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonNode.Parse(strResponse);
            string orderId = jsonResponse?["id"]?.ToString() ?? "";

            return Ok(new { id = orderId });
        }


        // complete Order mvc



        // ✅ الدالة الرئيسية لتحويل الطلب
        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder([FromBody] JsonObject data)
        {
            var orderId = data?["orderID"]?.ToString();

            if (string.IsNullOrEmpty(orderId))
                return BadRequest("Missing order ID");

            string accessToken = await GetPaypalAccessTokenInternal();
            string url = $"{_url}/v2/checkout/orders/{orderId}/capture";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

            var strResponse = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonNode.Parse(strResponse);

            string status = jsonResponse?["status"]?.ToString() ?? "";
            if (status == "COMPLETED")
            {
                var transaction = new PaymentTransaction
                {
                    PayPalOrderId = orderId,
                    Status = status,
                    Amount = decimal.Parse(jsonResponse?["purchase_units"]?[0]?["amount"]?["value"]?.ToString() ?? "0"),
                    CurrencyCode = jsonResponse?["purchase_units"]?[0]?["amount"]?["currency_code"]?.ToString() ?? "USD",
                    PayerEmail = jsonResponse?["payer"]?["email_address"]?.ToString() ?? ""
                };

                _context.PaymentTransactions.Add(transaction);
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", transactionId = transaction.Id });
            }

            return BadRequest(new { status = "error", details = strResponse });
        }
        private async Task<string> GetPaypalAccessTokenInternal()
        {
            string tokenUrl = $"{_url}/v1/oauth2/token";
            using var client = new HttpClient();

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_secret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.PostAsync(tokenUrl, content);

            if (!response.IsSuccessStatusCode)
                return "";

            var strResponse = await response.Content.ReadAsStringAsync();
            var json = JsonNode.Parse(strResponse);
            return json?["access_token"]?.ToString() ?? "";
        }




        [HttpGet("GetAccessToken")]
        public async Task<IActionResult> GetPaypalAccessToken()
        {
            var accessToken = await GetPaypalAccessTokenInternal();
            if (string.IsNullOrEmpty(accessToken))
                return BadRequest("Failed to get access token");

            return Ok(new { access_token = accessToken });
        }







    }
}

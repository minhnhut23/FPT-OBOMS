using BusinessObject.DTOs.PaymentDTO;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class MoMoService
{
    private readonly string _partnerCode = "MOMO";
    private readonly string _accessKey = "F8BBA842ECF85";
    private readonly string _secretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";
    private readonly string _endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";

    private readonly HttpClient _httpClient;

    public MoMoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MoMoResponseDTO> CreateMoMoPayment(PaymentRequestDTO request)
    {
        var orderId = Guid.NewGuid().ToString();
        var requestId = Guid.NewGuid().ToString();
        var returnUrl = "https://www.youtube.com/watch?v=mlIkPH0ehaE";
        var notifyUrl = "http://localhost:5000/api/payment/momo-callback";

        // Tạo rawData đúng thứ tự
        var rawData = $"accessKey={_accessKey}"
                    + $"&amount={request.Amount}"
                    + $"&extraData="
                    + $"&ipnUrl={notifyUrl}"
                    + $"&orderId={orderId}"
                    + $"&orderInfo=Thanh toán MoMo"
                    + $"&partnerCode={_partnerCode}"
                    + $"&redirectUrl={returnUrl}"
                    + $"&requestId={requestId}"
                    + $"&requestType=captureWallet";

        // Tạo chữ ký SHA256
        var signature = CreateSignature(rawData, _secretKey);

        var paymentRequest = new
        {
            partnerCode = _partnerCode,
            accessKey = _accessKey,
            requestId = requestId,
            amount = request.Amount,
            orderId = orderId,
            orderInfo = "Thanh toán MoMo",
            returnUrl = returnUrl,
            redirectUrl = returnUrl,
            notifyUrl = notifyUrl,
            ipnUrl = notifyUrl,
            requestType = "captureWallet",
            extraData = "",
            lang = "en",
            signature = signature
        };

        var jsonRequest = JsonConvert.SerializeObject(paymentRequest);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_endpoint, content);
        var jsonResponse = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<MoMoResponseDTO>(jsonResponse);
    }

    private static string CreateSignature(string rawData, string secretKey)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
        {
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}

public class MoMoResponseDTO
{
    [JsonProperty("partnerCode")]
    public string PartnerCode { get; set; }

    [JsonProperty("orderId")]
    public string OrderId { get; set; }

    [JsonProperty("requestId")]
    public string RequestId { get; set; }

    [JsonProperty("amount")]
    public long Amount { get; set; }

    [JsonProperty("responseTime")]
    public long ResponseTime { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("resultCode")]
    public int ResultCode { get; set; }

    [JsonProperty("payUrl")]
    public string PayUrl { get; set; }

    [JsonProperty("deeplink")]
    public string Deeplink { get; set; }

    [JsonProperty("qrCodeUrl")]
    public string QrCodeUrl { get; set; }

    [JsonProperty("extraData")]
    public string ExtraData { get; set; }
}

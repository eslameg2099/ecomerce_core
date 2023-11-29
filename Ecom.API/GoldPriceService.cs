using System.Net.Http;
using System.Threading.Tasks;

public class GoldPriceService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://www.goldapi.io/api/XAU/USD";
    private const string ApiKey = "goldapi-1gc96ukpwngg6v-io"; // Replace with your actual API key

    public GoldPriceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("x-access-token", ApiKey);
    }

    public async Task<string> GetGoldPriceAsync()
    {
        var response = await _httpClient.GetAsync(ApiUrl);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return "Error: Unable to retrieve gold price";
    }
}

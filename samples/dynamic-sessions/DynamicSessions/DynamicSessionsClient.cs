using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

using Azure.Core;

using DynamicSessions.Models;

using Microsoft.Extensions.Logging;

using OneOf;

namespace DynamicSessions;

public partial class DynamicSessionsClient
{
    private const string ApiVersion = "2024-02-02-preview";

    private readonly DynamicSessionsServiceOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TokenCredential _tokenCredential;
    private readonly ILogger<DynamicSessionsClient> _logger;

    private readonly Uri _endpointUri;

    public DynamicSessionsClient(
        DynamicSessionsServiceOptions options,
        IHttpClientFactory httpClientFactory,
        TokenCredential tokenCredential,
        ILogger<DynamicSessionsClient> logger)
    {
        _options = options;
        _httpClientFactory = httpClientFactory;
        _tokenCredential = tokenCredential;
        _logger = logger;

        _endpointUri = GetEndpointUri(options.Endpoint);
    }

    public async Task<OneOf<CodeExecutionResponse, DynamicSessionError>> ExecuteCodeAsync(CodeExecutionRequest request)
    {
        if (_options.SanitizeInput)
        {
            request = request with { PythonCode = SanitizeCode(request.PythonCode) };
        }

        _logger.LogTrace("Executing Dynamic Session Code: {Code}", request.PythonCode);

        using var httpClient = _httpClientFactory.CreateClient();

        await ConfigureHttpClientAsync(httpClient);

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"python/execute?api-version={ApiVersion}");

        var body = new { properties = request };
        requestMessage.Content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            "application/json");

        var response = await httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to execute code: {StatusCode}", response.StatusCode);
            var error = await response.Content.ReadAsStringAsync();

            return new DynamicSessionError
            {
                Message = error,
                Code = response.StatusCode.ToString()
            };
        }

        var text = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<CodeExecutionResponse>(text);

        if (responseContent is null)
        {
            _logger.LogError("Response content is null");

            return new DynamicSessionError
            {
                Message = "Response content is null",
                Code = "NullResponseContent"
            };
        }

        return responseContent;
    }

    public async Task<OneOf<UploadFileResponse, DynamicSessionError>> UploadFileAsync(UploadFileRequest request)
    {
        using var httpClient = _httpClientFactory.CreateClient();

        await ConfigureHttpClientAsync(httpClient);

        using var fileContent = await CreateFileContent(request.FileContent);
        using var requestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            $"files/upload?identifier={request.Identifier}&api-version={ApiVersion}");

        requestMessage.Content = new MultipartFormDataContent { { fileContent, "file", request.FileName } };

        var response = await httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to execute code: {StatusCode}", response.StatusCode);
            var error = await response.Content.ReadAsStringAsync();

            return new DynamicSessionError
            {
                Message = error,
                Code = response.StatusCode.ToString()
            };
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseElement = JsonSerializer.Deserialize<JsonElement>(responseContent);
        var valueProperties = responseElement.GetProperty("value")[0]
            .GetProperty("properties")
            .GetRawText();

        var uploadFileResponse = JsonSerializer.Deserialize<UploadFileResponse>(valueProperties);

        if (uploadFileResponse is null)
        {
            _logger.LogError("Response content is null");

            return new DynamicSessionError
            {
                Message = "Response content is null",
                Code = "NullResponseContent"
            };
        }

        return uploadFileResponse;
    }

    public async Task<OneOf<ListFilesResponse, DynamicSessionError>> ListFilesAsync(string identifier)
    {
        using var httpClient = _httpClientFactory.CreateClient();

        await ConfigureHttpClientAsync(httpClient);

        var response = await httpClient.GetAsync($"files?identifier={identifier}&api-version={ApiVersion}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to execute code: {StatusCode}", response.StatusCode);
            var error = await response.Content.ReadAsStringAsync();

            return new DynamicSessionError
            {
                Message = error,
                Code = response.StatusCode.ToString()
            };
        }

        var parsedResponse = await response.Content.ReadFromJsonAsync<ListFilesResponse>();

        if (parsedResponse is null)
        {
            _logger.LogError("Response content is null");

            return new DynamicSessionError
            {
                Message = "Response content is null",
                Code = "NullResponseContent"
            };
        }

        return parsedResponse;
    }

    private async Task ConfigureHttpClientAsync(HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Add(
            "User-Agent",
            $"Semantic-Kernel/{typeof(DynamicSessionsClient).Assembly.GetName().Version!.ToString()} (Language=dotnet)");

        var authHeaderValue = await _tokenCredential.GetTokenAsync(
            new TokenRequestContext(["https://dynamicsessions.io/.default"]),
            CancellationToken.None);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authHeaderValue.Token);

        httpClient.BaseAddress = _endpointUri;
    }

    private static async Task<ByteArrayContent> CreateFileContent(Stream fileContent)
    {
        using var memoryStream = new MemoryStream();
        await fileContent.CopyToAsync(memoryStream);

        var fileContentBytes = memoryStream.ToArray();

        return new ByteArrayContent(fileContentBytes);
    }

    private static string SanitizeCode(string code)
    {
        code = RemoveLeadingWhitespaceBackticksPython()
            .Replace(code, string.Empty);

        code = RemoveTrailingWhitespaceBackticks()
            .Replace(code, string.Empty);

        return code;
    }

    private static Uri GetEndpointUri(string endpoint)
    {
        if (endpoint.Contains("/python/execute"))
        {
            endpoint = endpoint.Replace("/python/execute", "");
        }

        if (!endpoint.EndsWith('/'))
        {
            endpoint += "/";
        }

        return new Uri(endpoint);
    }

    [GeneratedRegex(@"^(\s|`)*(?i:python)?\s*", RegexOptions.ExplicitCapture)]
    private static partial Regex RemoveLeadingWhitespaceBackticksPython();

    [GeneratedRegex(@"(\s|`)*$", RegexOptions.ExplicitCapture)]
    private static partial Regex RemoveTrailingWhitespaceBackticks();
}

public record DynamicSessionsServiceOptions
{
    public required string Endpoint { get; set; }

    public bool SanitizeInput { get; set; } = true;
}

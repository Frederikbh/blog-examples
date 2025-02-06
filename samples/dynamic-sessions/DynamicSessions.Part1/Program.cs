using DynamicSessions;
using DynamicSessions.Extensions;
using DynamicSessions.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");

var configuration = builder.Build();

var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(configuration);
services.AddHttpClient();
services.AddDynamicSessions();

var provider = services.BuildServiceProvider();

var client = provider.GetRequiredService<DynamicSessionsClient>();

const string SessionId = "session-id";

var uploadFileRequest = new UploadFileRequest
{
    Identifier = SessionId,
    FileName = "data.csv",
    FileContent = File.OpenRead("data.csv")
};
await client.UploadFileAsync(uploadFileRequest);

const string Code =
    "import pandas as pd\n\n# Load the data\nfile_path = '/mnt/data/data.csv'\ndata = pd.read_csv(file_path)\n\n# Display the data\nprint(data.head())";

var codeExecutionRequest = new CodeExecutionRequest
{
    Identifier = SessionId,
    PythonCode = Code
};
var result = await client.ExecuteCodeAsync(codeExecutionRequest);

var output = result.Match(success => success.Stdout, error => error.Message);

Console.WriteLine(output);

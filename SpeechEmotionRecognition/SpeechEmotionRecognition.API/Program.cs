using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpeechEmotionRecognition.API.Middleware;
using System.Text.Json;
using System.Text.Json.Serialization;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults((context, builder) =>
    {
        builder.UseMiddleware<ContentTypeValidationMiddleware>();
    })

    .ConfigureServices(s =>
    {
        s.Configure<JsonSerializerOptions>(options =>
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new JsonStringEnumConverter());
        });
    })
    .Build();

host.Run();
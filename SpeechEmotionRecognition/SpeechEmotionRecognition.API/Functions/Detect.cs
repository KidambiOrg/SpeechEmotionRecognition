using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SpeechEmotionRecognition.AI;
using SpeechEmotionRecognition.Model;
using static SpeechEmotionRecognition.Model.Enumerations;

namespace SpeechEmotionRecognition.API.Functions;

public class Detect
{
    private readonly ILogger _logger;
    private string _modelPath = $@"Model\audioFeatureModel.zip";

    public Detect(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Detect>();
    }

    [Function("Detect")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        _logger.LogInformation("Received audio file.");

        //Get query string
        object? includeFeatures;
        req.FunctionContext.BindingContext.BindingData.TryGetValue("includeFeatures", out includeFeatures);

        Stream audio = req.Body;

        //Predict from the uploaded audio

        _logger.LogInformation("Detecting emotion from  audio file...");
        AudioFeaturesOutput audioFeaturesOutput = PredictionManager.Predict(audio, _modelPath);

        string predictedEmotion = Enum.GetName(typeof(Emotion), audioFeaturesOutput.PredictedLabel);
        _logger.LogInformation($"Bundling response. Predicted emotion: {predictedEmotion}");
        ResponseOutput responseOutput = new ResponseOutput()
        {
            EmotionName = predictedEmotion,
            Score = audioFeaturesOutput.Score.Max().ToString("0.0%")
        };

        //If include features is true in the query string , generate the audio features.
        if (includeFeatures != null && Convert.ToBoolean(includeFeatures.ToString()))
        {
            responseOutput.Features = audioFeaturesOutput.ToAudioFeature();
        }
        var response = req.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(responseOutput).ConfigureAwait(false);
        return response;
    }
}
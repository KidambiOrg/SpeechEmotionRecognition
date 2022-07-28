using Microsoft.ML;
using Microsoft.ML.Data;
using SpeechEmotionRecognition.AI.Utilities;
using SpeechEmotionRecognition.Model;

namespace SpeechEmotionRecognition.AI;

public static class PredictionManager
{
    private static PredictionEngine<AudioFeature, AudioFeaturesOutput> _predictionEngine;

    public static AudioFeaturesOutput Predict(Stream audio, string modelFileFullPath)
    {
        AudioFeature audioFeature = new AudioAnalyzer().ExtractFeature(audio);

        return Predict(audioFeature, modelFileFullPath);
    }

    public static AudioFeaturesOutput Predict(AudioFeature input, string modelFileFullPath)
    {
        if (_predictionEngine == null)
        {
            _predictionEngine = CreatePredictEngine(modelFileFullPath);
        }
        //var b = _predictionEngine.Predict(input);

        //var labelBuffer = new VBuffer<ReadOnlyMemory<char>>();
        //_predictionEngine.OutputSchema["Score"].Annotations.GetValue("SlotNames", ref labelBuffer);
        //var labels = labelBuffer.DenseValues().Select(l => l.ToString()).ToArray();

        return _predictionEngine.Predict(input);
    }

    private static PredictionEngine<AudioFeature, AudioFeaturesOutput> CreatePredictEngine(string modelFileFullPath)
    {
        var mlContext = new MLContext();
        ITransformer mlModel = mlContext.Model.Load(modelFileFullPath, out var _);
        return mlContext.Model.CreatePredictionEngine<AudioFeature, AudioFeaturesOutput>(mlModel);
    }
}
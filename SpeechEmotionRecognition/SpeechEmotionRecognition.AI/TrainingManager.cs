using CsvHelper;
using Microsoft.ML;
using Microsoft.ML.Trainers.LightGbm;
using SpeechEmotionRecognition.Model;
using System.Globalization;

namespace SpeechEmotionRecognition.AI;

public class TrainingManager
{
    private MLContext _mlContext;

    public bool TrainModelAsync(string dataSourceFilePath, string modelFilePath)
    {
        IEnumerable<AudioFeature> audioFeatures = Enumerable.Empty<AudioFeature>();
        using (var reader = new StreamReader(dataSourceFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            audioFeatures = csv.GetRecords<AudioFeature>().ToList();
        }

        _mlContext = new MLContext();

        try
        {
            var data = _mlContext.Data.LoadFromEnumerable<AudioFeature>(audioFeatures);

            var pipeline = BuildPipeline();
            var model = pipeline.Fit(data);

            //Save model
            _mlContext.Model.Save(model, data.Schema, modelFilePath);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    internal IEstimator<ITransformer> BuildPipeline()
    {
        // Data process configuration with pipeline data transformations

        var options = new LightGbmMulticlassTrainer.Options()
        {
            NumberOfLeaves = 13,
            NumberOfIterations = 79,

            MinimumExampleCountPerLeaf = 38,
            LearningRate = 0.252316046821136,

            LabelColumnName = @"Emotion",
            FeatureColumnName = @"Features",
            ExampleWeightColumnName = null,
            Booster = new GradientBooster.Options()
            {
                SubsampleFraction = 0.270344053861829,
                FeatureFraction = 0.633552118398198,
                L1Regularization = 2.25343203409963E-05,
                L2Regularization = 0.0122098954322872
            },
            MaximumBinCountPerFeature = 142
        };

        var pipeline = _mlContext.Transforms.ReplaceMissingValues(new[]
                                    { new InputOutputColumnPair(@"Mfcc", @"Mfcc"),
                                        new InputOutputColumnPair(@"Chroma", @"Chroma"),
                                        new InputOutputColumnPair(@"MelSpectogram", @"MelSpectogram"),
                                        new InputOutputColumnPair(@"Centroid", @"Centroid"),
                                        new InputOutputColumnPair(@"Spread", @"Spread"),
                                        new InputOutputColumnPair(@"Flatness", @"Flatness"),
                                        new InputOutputColumnPair(@"Noiseness", @"Noiseness"),
                                        new InputOutputColumnPair(@"Rolloff", @"Rolloff"),
                                        new InputOutputColumnPair(@"Crest", @"Crest"),
                                        new InputOutputColumnPair(@"Entropy", @"Entropy"),
                                        new InputOutputColumnPair(@"Decrease", @"Decrease"),
                                        new InputOutputColumnPair(@"Contrast", @"Contrast"),
                                        new InputOutputColumnPair(@"Pitch", @"Pitch"),
                                        new InputOutputColumnPair(@"Energy", @"Energy"),
                                        new InputOutputColumnPair(@"Rms", @"Rms"),
                                        new InputOutputColumnPair(@"TimeEntropy", @"TimeEntropy"),
                                        new InputOutputColumnPair(@"Zcr", @"Zcr")
                                    })
                                .Append(_mlContext.Transforms.Concatenate(@"Features", new[] { @"Mfcc", @"Chroma", @"MelSpectogram", @"Centroid", @"Spread", @"Flatness", @"Noiseness", @"Rolloff", @"Crest", @"Entropy", @"Decrease", @"Contrast", @"Pitch", @"Energy", @"Rms", @"TimeEntropy", @"Zcr" }))
                                .Append(_mlContext.Transforms.Conversion.MapValueToKey(
                                    outputColumnName: @"Emotion",
                                    inputColumnName: @"Emotion"))
                                .Append(_mlContext.MulticlassClassification.Trainers.LightGbm(options))
                                .Append(_mlContext.Transforms.Conversion.MapKeyToValue(
                                    outputColumnName: @"PredictedLabel",
                                    inputColumnName: @"PredictedLabel"));

        return pipeline;
    }
}
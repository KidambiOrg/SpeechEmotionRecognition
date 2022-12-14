// This file was auto-generated by ML.NET Model Builder.
using Microsoft.ML.Trainers.LightGbm;
using Microsoft.ML;

namespace GenerateDataFromDataaset
{
    public partial class MLModel1
    {
        public static ITransformer RetrainPipeline(MLContext context, IDataView trainData)
        {
            var pipeline = BuildPipeline(context);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.ReplaceMissingValues(new[] { new InputOutputColumnPair(@"Mfcc", @"Mfcc"), new InputOutputColumnPair(@"Chroma", @"Chroma"), new InputOutputColumnPair(@"MelSpectogram", @"MelSpectogram"), new InputOutputColumnPair(@"Centroid", @"Centroid"), new InputOutputColumnPair(@"Spread", @"Spread"), new InputOutputColumnPair(@"Flatness", @"Flatness"), new InputOutputColumnPair(@"Noiseness", @"Noiseness"), new InputOutputColumnPair(@"Rolloff", @"Rolloff"), new InputOutputColumnPair(@"Crest", @"Crest"), new InputOutputColumnPair(@"Entropy", @"Entropy"), new InputOutputColumnPair(@"Decrease", @"Decrease"), new InputOutputColumnPair(@"Contrast", @"Contrast"), new InputOutputColumnPair(@"Pitch", @"Pitch"), new InputOutputColumnPair(@"Energy", @"Energy"), new InputOutputColumnPair(@"Rms", @"Rms"), new InputOutputColumnPair(@"TimeEntropy", @"TimeEntropy"), new InputOutputColumnPair(@"Zcr", @"Zcr") })
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new[] { @"Mfcc", @"Chroma", @"MelSpectogram", @"Centroid", @"Spread", @"Flatness", @"Noiseness", @"Rolloff", @"Crest", @"Entropy", @"Decrease", @"Contrast", @"Pitch", @"Energy", @"Rms", @"TimeEntropy", @"Zcr" }))
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: @"Emotion", inputColumnName: @"Emotion"))
                                    .Append(mlContext.MulticlassClassification.Trainers.LightGbm(new LightGbmMulticlassTrainer.Options() { NumberOfLeaves = 13, NumberOfIterations = 79, MinimumExampleCountPerLeaf = 38, LearningRate = 0.252316046821136, LabelColumnName = @"Emotion", FeatureColumnName = @"Features", ExampleWeightColumnName = null, Booster = new GradientBooster.Options() { SubsampleFraction = 0.270344053861829, FeatureFraction = 0.633552118398198, L1Regularization = 2.25343203409963E-05, L2Regularization = 0.0122098954322872 }, MaximumBinCountPerFeature = 142 }))
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: @"PredictedLabel", inputColumnName: @"PredictedLabel"));

            return pipeline;
        }
    }
}
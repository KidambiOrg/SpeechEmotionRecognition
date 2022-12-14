// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;
namespace GenerateDataFromDataaset
{
    public partial class MLModel1
    {
        /// <summary>
        /// model input class for MLModel1.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"Mfcc")]
            public float Mfcc { get; set; }

            [ColumnName(@"Chroma")]
            public float Chroma { get; set; }

            [ColumnName(@"MelSpectogram")]
            public float MelSpectogram { get; set; }

            [ColumnName(@"Centroid")]
            public float Centroid { get; set; }

            [ColumnName(@"Spread")]
            public float Spread { get; set; }

            [ColumnName(@"Flatness")]
            public float Flatness { get; set; }

            [ColumnName(@"Noiseness")]
            public float Noiseness { get; set; }

            [ColumnName(@"Rolloff")]
            public float Rolloff { get; set; }

            [ColumnName(@"Crest")]
            public float Crest { get; set; }

            [ColumnName(@"Entropy")]
            public float Entropy { get; set; }

            [ColumnName(@"Decrease")]
            public float Decrease { get; set; }

            [ColumnName(@"Contrast")]
            public float Contrast { get; set; }

            [ColumnName(@"Pitch")]
            public float Pitch { get; set; }

            [ColumnName(@"Energy")]
            public float Energy { get; set; }

            [ColumnName(@"Rms")]
            public float Rms { get; set; }

            [ColumnName(@"TimeEntropy")]
            public float TimeEntropy { get; set; }

            [ColumnName(@"Zcr")]
            public float Zcr { get; set; }

            [ColumnName(@"Emotion")]
            public float Emotion { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for MLModel1.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName(@"Mfcc")]
            public float Mfcc { get; set; }

            [ColumnName(@"Chroma")]
            public float Chroma { get; set; }

            [ColumnName(@"MelSpectogram")]
            public float MelSpectogram { get; set; }

            [ColumnName(@"Centroid")]
            public float Centroid { get; set; }

            [ColumnName(@"Spread")]
            public float Spread { get; set; }

            [ColumnName(@"Flatness")]
            public float Flatness { get; set; }

            [ColumnName(@"Noiseness")]
            public float Noiseness { get; set; }

            [ColumnName(@"Rolloff")]
            public float Rolloff { get; set; }

            [ColumnName(@"Crest")]
            public float Crest { get; set; }

            [ColumnName(@"Entropy")]
            public float Entropy { get; set; }

            [ColumnName(@"Decrease")]
            public float Decrease { get; set; }

            [ColumnName(@"Contrast")]
            public float Contrast { get; set; }

            [ColumnName(@"Pitch")]
            public float Pitch { get; set; }

            [ColumnName(@"Energy")]
            public float Energy { get; set; }

            [ColumnName(@"Rms")]
            public float Rms { get; set; }

            [ColumnName(@"TimeEntropy")]
            public float TimeEntropy { get; set; }

            [ColumnName(@"Zcr")]
            public float Zcr { get; set; }

            [ColumnName(@"Emotion")]
            public uint Emotion { get; set; }

            [ColumnName(@"Features")]
            public float[] Features { get; set; }

            [ColumnName(@"PredictedLabel")]
            public float PredictedLabel { get; set; }

            [ColumnName(@"Score")]
            public float[] Score { get; set; }

        }

        #endregion

        private static string MLNetModelPath = Path.GetFullPath("MLModel1.zip");

        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }
    }
}

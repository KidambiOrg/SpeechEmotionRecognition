using Microsoft.ML.Data;

namespace SpeechEmotionRecognition.Model;

public class AudioFeaturesOutput
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
    public Int32 PredictedLabel { get; set; }

    [ColumnName(@"Score")]
    public float[] Score { get; set; }
}
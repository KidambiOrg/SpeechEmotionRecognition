using Newtonsoft.Json;

namespace SpeechEmotionRecognition.Model;

public class AudioFeature
{
    [JsonProperty("mfcc")]
    public float Mfcc { get; set; }

    [JsonProperty("chroma")]
    public float Chroma { get; set; }

    [JsonProperty("mel")]
    public float MelSpectogram { get; set; }

    [JsonProperty("centroid")]
    public float Centroid { get; set; }

    [JsonProperty("spread")]
    public float Spread { get; set; }

    [JsonProperty("flatness")]
    public float Flatness { get; set; }

    [JsonProperty("noiseness")]
    public float Noiseness { get; set; }

    [JsonProperty("rolloff")]
    public float Rolloff { get; set; }

    [JsonProperty("crest")]
    public float Crest { get; set; }

    [JsonProperty("entropy")]
    public float Entropy { get; set; }

    [JsonProperty("decrease")]
    public float Decrease { get; set; }

    [JsonProperty("contrast")]
    public float Contrast { get; set; }

    [JsonProperty("pitch")]
    public float Pitch { get; set; }

    [JsonProperty("energy")]
    public float Energy { get; set; }

    [JsonProperty("rms")]
    public float Rms { get; set; }

    [JsonProperty("time_entropy")]
    public float TimeEntropy { get; set; }

    [JsonProperty("zcr")]
    public float Zcr { get; set; }

    [JsonProperty("emotion")]
    public int Emotion { get; set; }
}
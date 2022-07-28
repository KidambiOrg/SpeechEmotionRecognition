namespace SpeechEmotionRecognition.Model;

public record ResponseOutput
{
    public string EmotionName { get; set; }

    public string Score { get; set; }

    public AudioFeature Features { get; set; }
}
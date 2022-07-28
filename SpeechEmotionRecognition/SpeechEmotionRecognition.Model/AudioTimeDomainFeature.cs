namespace SpeechEmotionRecognition.Model
{
    public record AudioTimeDomainFeature
    {
        public float Energy { get; set; }

        public float Rms { get; set; }

        public float TimeEntropy { get; set; }

        public float Zcr { get; set; }
    }
}
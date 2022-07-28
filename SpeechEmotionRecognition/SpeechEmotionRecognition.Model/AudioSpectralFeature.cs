namespace SpeechEmotionRecognition.Model;

public record AudioSpectralFeature
{
    public float SpectralCentroid { get; set; }

    public float SpectralSpread { get; set; }

    public float SpectralFlatness { get; set; }

    public float SpectralNoiseness { get; set; }

    public float SpectralRolloff { get; set; }

    public float SpectralCrest { get; set; }
    public float SpectralEntropy { get; set; }

    public float SpectralDecrease { get; set; }

    public float SpectralContrast { get; set; }
}
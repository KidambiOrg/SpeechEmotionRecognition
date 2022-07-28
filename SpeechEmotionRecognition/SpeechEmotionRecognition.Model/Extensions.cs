namespace SpeechEmotionRecognition.Model
{
    public static class Extensions
    {
        public static AudioFeature ToAudioFeature(this AudioFeaturesOutput audioFeaturesOutput)
        {
            return new AudioFeature()
            {
                Emotion = Convert.ToInt32(audioFeaturesOutput.PredictedLabel),
                Mfcc = audioFeaturesOutput.Mfcc,
                Chroma = audioFeaturesOutput.Chroma,
                MelSpectogram = audioFeaturesOutput.MelSpectogram,
                Centroid = audioFeaturesOutput.Centroid,
                Spread = audioFeaturesOutput.Spread,
                Flatness = audioFeaturesOutput.Flatness,
                Noiseness = audioFeaturesOutput.Noiseness,
                Rolloff = audioFeaturesOutput.Rolloff,
                Crest = audioFeaturesOutput.Crest,
                Entropy = audioFeaturesOutput.Entropy,
                Decrease = audioFeaturesOutput.Decrease,
                Contrast = audioFeaturesOutput.Contrast,
                Pitch = audioFeaturesOutput.Pitch,
                Energy = audioFeaturesOutput.Energy,
                Rms = audioFeaturesOutput.Rms,
                TimeEntropy = audioFeaturesOutput.TimeEntropy,
                Zcr = audioFeaturesOutput.Zcr
            };
        }
    }
}
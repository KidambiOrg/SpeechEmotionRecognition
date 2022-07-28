using NWaves.Audio;
using NWaves.FeatureExtractors;
using NWaves.FeatureExtractors.Multi;
using NWaves.FeatureExtractors.Options;
using NWaves.Filters.Fda;
using NWaves.Signals;
using NWaves.Windows;
using SpeechEmotionRecognition.Model;

namespace SpeechEmotionRecognition.AI.Utilities;

public class AudioAnalyzer
{
    private readonly bool _includeMfcc;
    private readonly bool _includeSpectralFeatures;
    private readonly bool _includeChroma;
    private readonly bool _includeMelSpectrogram;
    private readonly bool _includePitch;
    private readonly bool _includeTimeDomainFeature;

    public AudioAnalyzer(bool includeMfcc = true
                        , bool includeSpectralFeatures = true
                        , bool includeChroma = true
                        , bool includeMelSpectogram = true
                        , bool includePitch = true
                        , bool includeTimeDomainFeature = true)
    {
        _includeChroma = includeChroma;
        _includeMfcc = includeMfcc;
        _includeMelSpectrogram = includeMelSpectogram;
        _includePitch = includePitch;
        _includeSpectralFeatures = includeSpectralFeatures;
        _includeTimeDomainFeature = includeTimeDomainFeature;
    }

    public AudioFeature ExtractFeature(string audioFilePath)
    {
        if (!File.Exists(audioFilePath))
        {
            return null;
        }

        WaveFile audioFile;
        using (var stream = new FileStream(audioFilePath, FileMode.Open))
        {
            audioFile = new WaveFile(stream);
        }
        return ExtractFeature(audioFile);
    }

    public AudioFeature ExtractFeature(Stream audioStream)
    {
        return ExtractFeature(new WaveFile(audioStream));
    }

    public AudioFeature ExtractFeature(WaveFile audioFile)
    {
        DiscreteSignal? signal = audioFile[Channels.Left];

        AudioFeature audioFeature = new AudioFeature();
        if (_includeMfcc)
        {
            audioFeature.Mfcc = GetMfcc(signal);
        }

        if (_includeChroma)
        {
            audioFeature.Chroma = GetChroma(signal);
        }

        if (_includeMelSpectrogram)
        {
            audioFeature.MelSpectogram = GetMelSpectogram(signal);
        }

        if (_includeSpectralFeatures)
        {
            AudioSpectralFeature audioSpectralFeature = GetSpectralFeatures(signal);

            audioFeature.Centroid = audioSpectralFeature.SpectralCentroid;

            audioFeature.Spread = audioSpectralFeature.SpectralSpread;

            audioFeature.Flatness = audioSpectralFeature.SpectralFlatness;

            audioFeature.Noiseness = audioSpectralFeature.SpectralNoiseness;

            audioFeature.Rolloff = audioSpectralFeature.SpectralRolloff;

            audioFeature.Crest = audioSpectralFeature.SpectralCrest;
            audioFeature.Entropy = audioSpectralFeature.SpectralEntropy;

            audioFeature.Decrease = audioSpectralFeature.SpectralDecrease;

            audioFeature.Contrast = audioSpectralFeature.SpectralContrast;
        }

        if (_includeTimeDomainFeature)
        {
            AudioTimeDomainFeature audioTimeDomainFeature = GetAudioTimeDomainFeature(signal);
            audioFeature.Energy = audioTimeDomainFeature.Energy;
            audioFeature.Rms = audioTimeDomainFeature.Rms;
            audioFeature.TimeEntropy = audioTimeDomainFeature.TimeEntropy;
            audioFeature.Zcr = audioTimeDomainFeature.Zcr;
        }

        if (_includePitch)
        {
            audioFeature.Pitch = GetPitch(signal);
        }

        return audioFeature;
    }

    private float GetMfcc(DiscreteSignal signal)
    {
        int sr = signal.SamplingRate;
        int fftSize = 2048;
        var melCount = 24;
        int filterbankSize = 128;

        //float[][] melBank = FilterBanks.MelBankSlaney(filterbankSize, fftSize, sr);

        (double, double, double)[]? freqs = FilterBanks.MelBands(melCount, sr);
        float[][]? fbank = FilterBanks.Triangular(fftSize, sr, freqs);

        FilterBanks.Normalize(melCount, freqs, fbank);

        var mfccOptions = new MfccOptions
        {
            SamplingRate = sr,
            FeatureCount = 13,
            // FrameDuration = 0.025,
            // HopDuration = 0.015/*sec*/,
            //FilterBankSize = 26,
            NonLinearity = NonLinearityType.ToDecibel,
            LogFloor = 1e-10f,
            // PreEmphasis = 0.97,
            DctType = "2N",
            FftSize = fftSize,
            Window = WindowType.Hann,
            SpectrumType = SpectrumType.Power,
            HopSize = 512,
            FilterBank = fbank

            //...unspecified parameters will have default values
        };
        MfccExtractor mfccExtractor = new MfccExtractor(mfccOptions);
        List<float[]> mfccVectors = mfccExtractor.ParallelComputeFrom(signal);
        mfccExtractor.Reset();
        List<float> mfccFlatten = new List<float>();

        for (int m = 1; m < 13; m++)
        {
            for (int hop = 20; hop > 0; hop--)
            {
                mfccFlatten.Add(mfccVectors[20 - hop][m]);
            }
        }
        var mfcc = mfccFlatten.Average();

        return mfcc;
    }

    private float GetChroma(DiscreteSignal signal)
    {
        var options = new ChromaOptions
        {
            // basic parameters:

            SamplingRate = 16000,
            FrameSize = 2048,
            HopSize = 512,

            // additional parameters (usually no need to change their default values):

            FeatureCount = 12,
            Tuning = 0,
            CenterOctave = 5.0,
            OctaveWidth = 2,
            Norm = 2,
            BaseC = true
        };

        var chromaExtractor = new ChromaExtractor(options);
        var chromaVectors = chromaExtractor.ComputeFrom(signal);
        chromaExtractor.Reset();
        var chromaFlatten = new List<float>();

        for (int m = 0; m < 12; m++)
        {
            for (int hop = 20; hop > 0; hop--)
            {
                chromaFlatten.Add(chromaVectors[20 - hop][m]);
            }
        }
        var chroma = chromaFlatten.Average();
        return chroma;
    }

    private float GetMelSpectogram(DiscreteSignal signal)
    {
        var hopSize = 512;
        int sr = signal.SamplingRate;
        int fftSize = 1024;
        var melCount = 40;

        var melExtractor = new FilterbankExtractor(
           new FilterbankOptions
           {
               SamplingRate = sr,
               FrameSize = fftSize,
               FftSize = fftSize,
               HopSize = hopSize,
               Window = WindowType.Hann,
               FilterBank = FilterBanks.Triangular(fftSize, sr, FilterBanks.MelBands(melCount, sr)),
           });

        var melSpectrogram = melExtractor.ComputeFrom(signal);
        var melFlatten = new List<float>();
        for (int m = 1; m < 13; m++)
        {
            for (int hop = 20; hop > 0; hop--)
            {
                melFlatten.Add(melSpectrogram[20 - hop][m]);
            }
        }
        var mel = melFlatten.Average();
        return mel;
    }

    private AudioTimeDomainFeature GetAudioTimeDomainFeature(DiscreteSignal signal)
    {
        //Spectral Contrast
        int sr = signal.SamplingRate;
        var freqs = new float[] { 80, 100, 120, 140, 150, 200, 300, 500, 800, 1200, 1600, 2500, 5000/*Hz*/ };
        var timeDomainOptions = new MultiFeatureOptions
        {
            SamplingRate = sr,
            Frequencies = freqs,
            FeatureList = "all"
        };

        var tdExtractor = new TimeDomainFeaturesExtractor(timeDomainOptions);
        List<float[]> tdVectors = tdExtractor.ParallelComputeFrom(signal);
        tdExtractor.Reset();

        //Energy
        AudioTimeDomainFeature audioTimeDomainFeature = new AudioTimeDomainFeature()
        {
            Energy = GetVectorAverageValues(tdVectors, 0),
            Rms = GetVectorAverageValues(tdVectors, 1),
            TimeEntropy = GetVectorAverageValues(tdVectors, 2),
            Zcr = GetVectorAverageValues(tdVectors, 3)
        };

        return audioTimeDomainFeature;
    }

    private AudioSpectralFeature GetSpectralFeatures(DiscreteSignal signal)
    {
        //Spectral Contrast
        int sr = signal.SamplingRate;
        var freqs = new float[] { 80, 100, 120, 140, 150, 200, 300, 500, 800, 1200, 1600, 2500, 5000/*Hz*/ };
        var contrastOptions = new MultiFeatureOptions
        {
            SamplingRate = sr,
            Frequencies = freqs
        };
        var constrastExtractor = new SpectralFeaturesExtractor(contrastOptions);
        List<float[]> constrastVectors = constrastExtractor.ParallelComputeFrom(signal);
        constrastExtractor.Reset();

        var spectralFeaturesFlatten = new List<float>();

        //Contrast
        for (int m = 0; m < constrastVectors.Count - 1; m++)
        {
            for (int j = 8; j < 14; j++)
                if (!float.IsNaN(constrastVectors[m][j]))
                {
                    spectralFeaturesFlatten.Add(constrastVectors[m][j]);
                }
        }
        var contrast = spectralFeaturesFlatten.Average();

        if (float.IsInfinity(contrast))
        {
            contrast = 0;
        }

        //centroid
        float centroid = GetVectorAverageValues(constrastVectors, 0);

        //Spread
        float spread = GetVectorAverageValues(constrastVectors, 1);

        //flatness
        float flatness = GetVectorAverageValues(constrastVectors, 2);

        //noiseness
        float noiseness = GetVectorAverageValues(constrastVectors, 3);

        //rolloff
        float rolloff = GetVectorAverageValues(constrastVectors, 4);

        //crest
        float crest = GetVectorAverageValues(constrastVectors, 5);

        //entropy
        float entropy = GetVectorAverageValues(constrastVectors, 6);

        //decrease
        float decrease = GetVectorAverageValues(constrastVectors, 7);

        AudioSpectralFeature spectralFeature = new AudioSpectralFeature()
        {
            SpectralCentroid = centroid,
            SpectralSpread = spread,
            SpectralContrast = contrast,
            SpectralNoiseness = noiseness,
            SpectralCrest = crest,
            SpectralDecrease = decrease,
            SpectralEntropy = entropy,
            SpectralFlatness = flatness,
            SpectralRolloff = rolloff
        };

        return spectralFeature;
    }

    private float GetPitch(DiscreteSignal signal)
    {
        int sr = signal.SamplingRate;

        // 12 overlapping mel bands in frequency range [0, 4200] Hz
        (double, double, double)[]? melBands = FilterBanks.MelBands(12, sr, 0, 4200, true);
        var pitchOpts = new PitchOptions
        {
            SamplingRate = sr,
            FrameDuration = 0.04,
            HopDuration = 0.015,
            LowFrequency = 80/*Hz*/,
            HighFrequency = 5000/*Hz*/
        };
        var pitchExtractor = new PitchExtractor(pitchOpts);

        var pitches = pitchExtractor.ComputeFrom(signal).Select(item => item[0]).ToArray();
        pitchExtractor.Reset();

        var pitch = pitches.Average();

        return pitch;
    }

    private float GetVectorAverageValues(List<float[]> vectors, int indexToUse)
    {
        List<float> spectralFeaturesFlatten = new List<float>();
        for (int m = 0; m < vectors.Count - 1; m++)
        {
            if (!float.IsNaN(vectors[m][indexToUse]))
            {
                spectralFeaturesFlatten.Add(vectors[m][indexToUse]);
            }
        }
        var feature = spectralFeaturesFlatten.Average();

        if (float.IsInfinity(feature))
        {
            feature = 0;
        }

        return feature;
    }
}
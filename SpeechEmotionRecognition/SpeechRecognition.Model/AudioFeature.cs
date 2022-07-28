using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechRecognition.Model;

public class AudioFeature
{
    public float Mfcc { get; set; }

    public float Chroma { get; set; }

    public float MelSpectogram { get; set; }

    public AudioSpectralFeature SpectralFeatures { get; set; } = new AudioSpectralFeature();

    public float Pitch { get; set; }

    public int? Emotion { get; set; }
}
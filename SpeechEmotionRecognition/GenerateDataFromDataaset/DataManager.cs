using SpeechEmotionRecognition.AI.Utilities;
using SpeechEmotionRecognition.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDataFromDataaset
{
    internal static class DataManager
    {
        private static string _testOutPutData = @"C:\Kidambi\WorkRelated\SpeechEmotionRecognition\Test\test.csv";
        private static string _testDirectory = @"C:\Kidambi\WorkRelated\SpeechEmotionRecognition\test";
        private static string _sourceOutputData = $@"C:\Kidambi\WorkRelated\SpeechEmotionRecognition\Dataset\dataset.csv";

        private static ConcurrentBag<AudioFeature> _audioFeatures;
        private static bool _isTest;

        internal static string GenerateData(bool isTest = false)
        {
            _isTest = isTest;
            _audioFeatures = new ConcurrentBag<AudioFeature>();

            var taskRavDesfiles = Task.Run(() => ParseRavDessFiles(_audioFeatures));

            if (_isTest)
            {
                Task.WhenAll(taskRavDesfiles).Wait();
            }
            else
            {
                var taskCremafiles = Task.Run(() => ParseCremaFiles(_audioFeatures));
                var taskTessfiles = Task.Run(() => ParseTessFiles(_audioFeatures));
                var taskSaveefiles = Task.Run(() => ParseSaveeFiles(_audioFeatures));
                Task.WhenAll(taskSaveefiles, taskRavDesfiles, taskCremafiles, taskTessfiles).Wait();
            }

            return WriteDataSet(_audioFeatures);
        }

        private static string WriteDataSet(IEnumerable<AudioFeature> audioFeatures)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Mfcc,Chroma,MelSpectogram,Centroid,Spread,Flatness,Noiseness,Rolloff,Crest,Entropy,Decrease,Contrast,Pitch,Energy,Rms,TimeEntropy,Zcr,Emotion");
            foreach (AudioFeature audioFeature in audioFeatures)
            {
                sb.AppendLine($"{audioFeature.Mfcc}" +
                    $",{audioFeature.Chroma}" +
                    $",{audioFeature.MelSpectogram}" +
                    $",{audioFeature.Centroid}" +
                    $",{audioFeature.Spread}" +
                    $",{audioFeature.Flatness}" +
                    $",{audioFeature.Noiseness}" +
                    $",{audioFeature.Rolloff}" +
                    $",{audioFeature.Crest}" +
                    $",{audioFeature.Entropy}" +
                    $",{audioFeature.Decrease}" +
                    $",{audioFeature.Contrast}" +
                    $",{audioFeature.Pitch}" +
                    $",{audioFeature.Energy}" +
                    $",{audioFeature.Rms}" +
                    $",{audioFeature.TimeEntropy}" +
                    $",{audioFeature.Zcr}" +
                    $",{audioFeature.Emotion}");
            }

            string outputFile = _sourceOutputData;
            if (_isTest)
            {
                outputFile = _testOutPutData;
            }

            File.WriteAllText(outputFile, sb.ToString());

            return outputFile;
        }

        private static void ParseRavDessFiles(ConcurrentBag<AudioFeature> audioFeatures)
        {
            //_testDirectory;
            string directory = @"C:\Kidambi\WorkRelated\SpeechEmotionRecognition\Dataset\Ravdess";
            if (_isTest)
            {
                directory = _testDirectory;
            }

            //Ravdess files

            Parallel.ForEach(Directory.GetDirectories(directory), dir =>
            {
                foreach (string file in Directory.GetFiles(dir, "*.wav"))
                {
                    Console.WriteLine($"Reading file {Path.GetFileNameWithoutExtension(file)}...");

                    var fileParts = Path.GetFileNameWithoutExtension(file).Split('-', StringSplitOptions.RemoveEmptyEntries);

                    AudioFeature audioFeature = new AudioAnalyzer().ExtractFeature(file);
                    audioFeature.Emotion = (_isTest) ? -1 : Convert.ToInt32(fileParts[2]);
                    audioFeatures.Add(audioFeature);
                }
            });
        }

        private static void ParseSaveeFiles(ConcurrentBag<AudioFeature> audioFeatures)
        {
            //Savee
            string saveeDirectory = @"C:\Kidambi\WorkRelated\SpeechEmotionRecognition\Dataset\Savee";
            Dictionary<string, int> saveeEmotions = new Dictionary<string, int>()
            {
                { "sa",04},
                { "a",05},
                { "d",07},
                { "f",06},
                { "h",03},
                { "n",01},
                {"su",08 }
            };

            Parallel.ForEach(Directory.GetFiles(saveeDirectory, "*.wav"), file =>
            {
                Console.WriteLine($"Reading file {Path.GetFileNameWithoutExtension(file)}...");

                var fileParts = Path.GetFileNameWithoutExtension(file).Split('_', StringSplitOptions.RemoveEmptyEntries);

                AudioFeature audioFeature = new AudioAnalyzer().ExtractFeature(file);

                audioFeature.Emotion = fileParts[1] switch
                {
                    string s when s.StartsWith("a", StringComparison.InvariantCultureIgnoreCase) => saveeEmotions["a"],
                    string s when s.StartsWith("sa", StringComparison.InvariantCultureIgnoreCase) => saveeEmotions["sa"],
                    string s when s.StartsWith("d", StringComparison.InvariantCultureIgnoreCase) => saveeEmotions["d"],
                    string s when s.StartsWith("f", StringComparison.InvariantCultureIgnoreCase) => saveeEmotions["f"],
                    string s when s.StartsWith("h", StringComparison.InvariantCultureIgnoreCase) => saveeEmotions["h"],
                    string s when s.StartsWith("n", StringComparison.InvariantCultureIgnoreCase) => saveeEmotions["n"],
                    string s when s.StartsWith("su", StringComparison.InvariantCultureIgnoreCase) => saveeEmotions["su"],

                    _ => -1
                };

                audioFeatures.Add(audioFeature);
            });
        }

        private static void ParseTessFiles(ConcurrentBag<AudioFeature> audioFeatures)
        {
            // Tess
            string tessDirectory = @"C:\Kidambi\WorkRelated\SpeechEmotionRecognition\Dataset\Tess";
            Dictionary<string, int> tessEmotions = new Dictionary<string, int>()
            {
                { "sad",04},
                { "angry",05},
                { "disgust",07},
                { "fear",06},
                { "happy",03},
                { "neutral",01},
                {"ps",08 }
            };

            Parallel.ForEach(Directory.GetDirectories(tessDirectory), dir =>
            {
                foreach (string file in Directory.GetFiles(dir, "*.wav"))
                {
                    Console.WriteLine($"Reading file {Path.GetFileNameWithoutExtension(file)}...");

                    var fileParts = Path.GetFileNameWithoutExtension(file).Split('_', StringSplitOptions.RemoveEmptyEntries);

                    AudioFeature audioFeature = new AudioAnalyzer().ExtractFeature(file);
                    audioFeature.Emotion = tessEmotions[fileParts[2]];
                    audioFeatures.Add(audioFeature);
                }
            });
        }

        private static void ParseCremaFiles(ConcurrentBag<AudioFeature> audioFeatures)
        {
            //Crema files
            string cremaDirectory = @"C:\Kidambi\WorkRelated\SpeechEmotionRecognition\Dataset\Crema";
            Dictionary<string, int> cremaEmotions = new Dictionary<string, int>()
            {
                { "SAD",04},
                { "ANG",05},
                { "DIS",07},
                { "FEA",06},
                { "HAP",03},
                { "NEU",01}
            };

            Parallel.ForEach(Directory.GetFiles(cremaDirectory, "*.wav"), file =>
            {
                Console.WriteLine($"Reading file {Path.GetFileNameWithoutExtension(file)}...");

                var fileParts = Path.GetFileNameWithoutExtension(file).Split('_', StringSplitOptions.RemoveEmptyEntries);

                AudioFeature audioFeature = new AudioAnalyzer().ExtractFeature(file);
                audioFeature.Emotion = cremaEmotions[fileParts[2]];
                audioFeatures.Add(audioFeature);
            });
        }
    }
}
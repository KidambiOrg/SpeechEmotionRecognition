using SpeechEmotionRecognition.AI;
using SpeechEmotionRecognition.AI.Utilities;
using SpeechEmotionRecognition.Model;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace GenerateDataFromDataaset // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static string _modelPath = $@"C:\Kidambi\WorkRelated\SpeechEmotionRecognition\SpeechEmotionRecognition\SpeechEmotionRecognition.API\Model\audioFeatureModel.zip";

        private static void Main(string[] args)

        {
            string outputDataFile = DataManager.GenerateData();
            Console.WriteLine();
            Console.WriteLine($"Done generating dataset. Now training the model.");

            bool isTrainingSuccess = new TrainingManager().TrainModel(outputDataFile, _modelPath);

            Console.WriteLine($"All done. Press any key to exit. Training status: {isTrainingSuccess}");
            Console.ReadKey();
        }
    }
}
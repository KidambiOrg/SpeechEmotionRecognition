# Speech Emotion Recognition (SER)
An API to detect emotion in an audio file. A Python implementation using Librosa is available as noted in the credits section.
However a c# / .NET implementation was needed and this API was developed.

This feature leverage ML.NET to train and predict properties of the audio file. This feature does not use Multi-layer Perceptron Classifier. It leverages Multi Class Classification using  `LightGbmMulticlassTrainer` algorithm.

*Note*: The explanation of the Audio properties was gotten from the site mentioned in the credit section.

## Audio Properties

-  **Mel scale** — deals with human perception of frequency, it is a scale of pitches judged by listeners to be equal distance from each other
-  **Pitch** — how high or low a sound is. It depends on frequency, higher pitch is high frequency
-  **Frequency** — speed of vibration of sound, measures wave cycles per second
-  **Chroma** — Representation for audio where spectrum is projected onto 12 bins representing the 12 distinct semitones (or chroma). Computed by summing the log frequency magnitude spectrum across octaves.
-  **MFCC** — Mel Frequency Cepstral Coefficients: Voice is dependent on the shape of vocal tract including tongue, teeth, etc. Representation of short-time power spectrum of sound, essentially a representation of the vocal tract.

## Project Structure

1. `GenerateDataFromDataset` - project to generate audio properties in a CSV file
2. `SpeechEmotionRecognition.AI` - Contains the ML.NET code to train the model and predict.
3. `SpeechEmotionRecognition.API` - An Azure Function that accepts and audio file and return the predictions along with the audio properties of the file
4. `SpeechEmotionRecognition.Model` - A .NET library to hold properties in form of classes

## Pre-Requisites
1. Visual Studio 2022
2. Male sure the `model.zip` file in the models sub-directory in the output folder. IF not exist, run the  `GenerateDataFromDataSet` console app to get the model and copy to the `models` folder.

## How To Run
1. Clone / Download the code.
2. Open VS2022 and press F5.
3. Open any REST Client and call the Azure Function endpoint and pass an audio file as input.

## Datasets
The data sets used in the model can be downloaded from [here](https://drive.google.com/file/d/1wWsrN2Ep7x6lWqOXfr4rpKGYrJhWc8z7/view)

## Audio Processing Library
[NWaves](https://github.com/ar1st0crat/NWaves) was used to extact Audio properties like MFCC, Chroma, Contrast and so on

## Credits

1. [Speech Emotion Recognition Applied Project](https://extrudesign.com/speech-emotion-recognition/)
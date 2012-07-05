using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Xna.Framework;
using Helper;

namespace MyGame
{
    /// <summary>
    /// This class represent Speech recognizer that recognize voice commands and fire the specific events 
    /// according to command recongized
    /// </summary>
    class SpeechRecognizer : GameComponent
    {
        /// <summary>
        /// Format of Kinect audio stream samples.
        /// </summary>
        private const EncodingFormat AudioFormat = EncodingFormat.Pcm;

        /// <summary>
        /// Samples per second in Kinect audio stream.
        /// </summary>
        private const int AudioSamplesPerSecond = 16000;

        /// <summary>
        /// Bits per audio sample in Kinect audio stream.
        /// </summary>
        private const int AudioBitsPerSample = 16;

        /// <summary>
        /// Number of channels in Kinect audio stream.
        /// </summary>
        private const int AudioChannels = 1;

        /// <summary>
        /// Average bytes per second in Kinect audio stream
        /// </summary>
        private const int AudioAverageBytesPerSecond = 32000;

        /// <summary>
        /// Block alignment in Kinect audio stream.
        /// </summary>
        private const int AudioBlockAlign = 2;

        /// <summary>
        /// Active Kinect sensor.
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        private SpeechRecognitionEngine speechEngine;

        private MyGame myGame;

        /// <summary>
        /// List of all UI span elements used to select recognized text.
        /// </summary>
        //private List<Span> recognitionSpans;

        public SpeechRecognizer(MyGame game):base(game)
        {
            myGame = game;
            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                try
                {
                    // Start the sensor!
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    // Some other application is streaming from the same Kinect sensor
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {
                return;
            }

            RecognizerInfo ri = GetKinectRecognizer();

            if (null != ri)
                this.speechEngine = new SpeechRecognitionEngine(ri.Id);
            else
                this.speechEngine = new SpeechRecognitionEngine();

                //// Create a grammar from grammar definition XML file.
                using (var memoryStream = new MemoryStream(File.ReadAllBytes("SpeechGrammar.xml")))
                {
                    var g = new Grammar(memoryStream);
                    speechEngine.LoadGrammar(g);
                }

                speechEngine.SpeechRecognized += SpeechRecognized;

                speechEngine.SetInputToAudioStream(
                    sensor.AudioSource.Start(), new SpeechAudioFormatInfo(AudioFormat, AudioSamplesPerSecond, AudioBitsPerSample, AudioChannels, AudioAverageBytesPerSecond, AudioBlockAlign, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
        }

        private static RecognizerInfo GetKinectRecognizer()
        {
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }

        /// <summary>
        /// Handler for recognized speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Speech utterance confidence below which we treat speech as if it hadn't been heard
            const double ConfidenceThreshold = 0.5;


            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                switch (e.Result.Semantics.Value.ToString())
                {
                    case "PAUSE":
                        Console.WriteLine("PAUSE");
                        myGame.mediator.fireEvent(MyEvent.G_PAUSE);
                        break;

                    case "RESUME":
                        Console.WriteLine("RESUME");
                        myGame.mediator.fireEvent(MyEvent.G_RESUME);
                        break;

                    case "START":
                        Console.WriteLine("START");
                        myGame.mediator.fireEvent(MyEvent.G_StartLevel);
                        myGame.mediator.fireEvent(MyEvent.G_StartGame);
                        break;

                    case "EXIT":
                        Console.WriteLine("EXIT");
                        myGame.mediator.fireEvent(MyEvent.G_Exit);
                        break;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != this.sensor)
                {
                    this.sensor.AudioSource.Stop();

                    this.sensor.Stop();
                    this.sensor = null;
                }

                if (null != this.speechEngine)
                {
                    this.speechEngine.SpeechRecognized -= SpeechRecognized;
                    //this.speechEngine.SpeechRecognitionRejected -= SpeechRejected;
                    this.speechEngine.RecognizeAsyncStop();
                }
            }
            base.Dispose(disposing);
        }


    }
}

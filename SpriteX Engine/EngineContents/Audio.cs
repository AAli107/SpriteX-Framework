using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace SpriteX_Engine.EngineContents
{
    public class Audio
    {

        private string fileName; // Stores the Sound File's name
        BlockAlignReductionStream stream = null; // Sound Data
        DirectSoundOut output = new DirectSoundOut(); // The Output Sound
        WaveFileReader wfr; 
        WaveStream pcm;

        public Audio(string fileName, bool playImmediately = true)
        {
            if (File.Exists(fileName))
            {
                if (fileName.EndsWith(".wav"))
                {
                    this.fileName = fileName;
                    wfr = new WaveFileReader(fileName);
                    pcm = new WaveChannel32(wfr);
                    stream = new BlockAlignReductionStream(pcm);
                    output.Init(stream);
                    pcm.Flush();
                    wfr.Flush();
                    stream.Flush();

                    if (playImmediately) Play();
                }
                else throw new Exception("Audio Type Unsupported! Only supports .wav audio files");
            }
        }

        public void Dispose()
        {
            if (pcm != null) { pcm.Flush(); pcm.Dispose(); pcm = null; }
            if (wfr != null) { wfr.Flush(); wfr.Dispose(); wfr = null; }
            if (stream != null) { stream.Flush(); stream.Dispose(); stream = null; }
            if (output != null) { output.Dispose(); output = null; }
        }

        /// <summary>
        /// Plays Audio
        /// </summary>
        public void Play()
        {
            if (output != null) output.Play();
        }

        /// <summary>
        /// Stops playing Audio
        /// </summary>
        public void Stop() 
        {
            if (output != null) output.Stop();
        }

        /// <summary>
        /// Returns true if audio stops playing
        /// </summary>
        /// <returns></returns>
        public bool IsStopped() 
        {
            if (File.Exists(fileName))
                return output.PlaybackPosition.TotalMilliseconds >= stream.TotalTime.TotalMilliseconds;
            else return true;
        }
    }
}

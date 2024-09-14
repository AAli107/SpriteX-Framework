using NAudio.Utils;
using NAudio.Wave;

namespace SpriteX_Engine.EngineContents
{
    public class Audio
    {
        string fileName;
        WaveStream wfr = null;
        BlockAlignReductionStream stream = null;
        WaveOutEvent output = new WaveOutEvent();
        double totalTime = 0;

        public Audio(string fileName, float volume = 1f)
        {
            if (File.Exists(fileName))
            {
                if (fileName.EndsWith(".wav") || fileName.EndsWith(".mp3"))
                {
                    this.fileName = fileName;
                    wfr = new WaveFileReader(fileName);
                    stream = new BlockAlignReductionStream(fileName.EndsWith(".wav") ? new WaveChannel32(wfr) : new Mp3FileReader(wfr));
                    output.DesiredLatency = 100;
                    output.Init(stream);
                    totalTime = stream.TotalTime.Milliseconds;
                    output.Volume = volume;
                    Play();
                }
                else throw new Exception("Audio Type Unsupported: Only supports .wav & .mp3 audio formats.");
            }
        }

        public void Dispose()
        {
            if (wfr != null) { wfr.Flush(); wfr.Dispose(); wfr.Close(); wfr = null; }
            if (stream != null) { stream.Flush(); stream.Dispose(); stream.Close(); stream = null; }
            if (output != null) { output.Dispose(); output = null; }
        }

        /// <summary>
        /// Plays Audio
        /// </summary>
        public void Play()
        {
            output?.Play();
        }

        /// <summary>
        /// Stops playing Audio
        /// </summary>
        public void Stop() 
        {
            output?.Stop();
        }

        /// <summary>
        /// Returns true if audio stops playing
        /// </summary>
        /// <returns></returns>
        public bool IsStopped() 
        {
            if (File.Exists(fileName))
                return output.GetPositionTimeSpan().TotalMilliseconds > totalTime;
            else return true;
        }
    }
}

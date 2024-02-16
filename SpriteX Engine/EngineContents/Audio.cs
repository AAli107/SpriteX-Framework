using NAudio.Wave;

namespace SpriteX_Engine.EngineContents
{
    public class Audio
    {

        private string fileName; // Stores the Sound File's name
        BlockAlignReductionStream stream = null; // Sound Data
        DirectSoundOut output = new DirectSoundOut(); // The Output Sound
        WaveStream wfr;
        Stream audioStream;
        double totalTime = 0;

        public Audio(string fileName, bool playImmediately = true)
        {
            if (File.Exists(fileName))
            {
                if (fileName.EndsWith(".wav") || fileName.EndsWith(".mp3"))
                {
                    this.fileName = fileName;
                    audioStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    wfr = new WaveFileReader(audioStream);
                    stream = new BlockAlignReductionStream(fileName.EndsWith(".wav") ? new WaveChannel32(wfr) : new Mp3FileReader(wfr));
                    output.Init(stream);
                    totalTime = stream.TotalTime.TotalMilliseconds;

                    if (playImmediately) Play();
                }
                else throw new Exception("Audio Type Unsupported: Only supports .wav & .mp3 audio formats.");
            }
        }

        public void Dispose()
        {
            if (audioStream != null) { audioStream.Flush(); audioStream.Dispose(); audioStream.Close(); audioStream = null; }
            if (wfr != null) { wfr.Flush(); wfr.Dispose(); wfr.Close(); wfr = null; }
            if (stream != null) { stream.Flush(); stream.Dispose(); stream.Close(); stream = null; }
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
                return output.PlaybackPosition.TotalMilliseconds >= totalTime;
            else return true;
        }
    }
}

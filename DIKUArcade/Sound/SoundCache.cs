namespace DIKUArcade.Sound;

using NAudio.Wave;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a cached sound, storing audio data and its wave format for efficient playback.
/// </summary>
public class CachedSound {
    /// <summary>
    /// Gets the audio data of the cached sound as an array of floats.
    /// </summary>
    public float[] AudioData {
        get; private set;
    }

    /// <summary>
    /// Gets the wave format of the cached sound.
    /// </summary>
    public WaveFormat WaveFormat {
        get; private set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedSound"/> class by loading audio data
    /// from the specified audio file.
    /// </summary>
    /// <param name="audioFileName">The path to the audio file to cache.</param>
    public CachedSound(string audioFileName) {
        using (var audioFileReader = new AudioFileReader(audioFileName)) {
            WaveFormat = audioFileReader.WaveFormat;
            var wholeFile = new List<float>((int) (audioFileReader.Length / 4));
            var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
            int samplesRead;
            while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0) {
                wholeFile.AddRange(readBuffer.Take(samplesRead));
            }
            AudioData = wholeFile.ToArray();
        }
    }
}
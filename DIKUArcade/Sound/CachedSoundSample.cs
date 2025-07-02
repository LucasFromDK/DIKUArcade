namespace DIKUArcade.Sound;

using System;
using NAudio.Wave;

/// <summary>
/// Provides audio samples from a cached sound for playback.
/// </summary>
public class CachedSoundSampleProvider : ISampleProvider {
    private readonly CachedSound cachedSound;
    private long position;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedSoundSampleProvider"/> class.
    /// </summary>
    /// <param name="cachedSound">The cached sound to provide samples from.</param>
    public CachedSoundSampleProvider(CachedSound cachedSound) {
        this.cachedSound = cachedSound;
    }

    /// <summary>
    /// Reads audio samples into the provided buffer.
    /// </summary>
    /// <param name="buffer">The buffer to fill with audio samples.</param>
    /// <param name="offset">The offset in the buffer to start writing samples.</param>
    /// <param name="count">The number of samples to read.</param>
    /// <returns>The number of samples actually read.</returns>
    public int Read(float[] buffer, int offset, int count) {
        var availableSamples = cachedSound.AudioData.Length - position;
        var samplesToCopy = Math.Min(availableSamples, count);
        Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
        position += samplesToCopy;
        return (int) samplesToCopy;
    }

    /// <summary>
    /// Gets the wave format of the cached sound.
    /// </summary>
    public WaveFormat WaveFormat {
        get {
            return cachedSound.WaveFormat;
        }
    }
}
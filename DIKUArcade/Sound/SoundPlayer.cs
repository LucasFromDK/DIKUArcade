namespace DIKUArcade.Sound;

using System;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

/// <summary>
/// A sound player that supports playing multiple sounds simultaneously using a mixer.
/// </summary>
public class SoundPlayer : IDisposable {
    private readonly IWavePlayer outputDevice;
    private readonly MixingSampleProvider mixer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundPlayer"/> class.
    /// </summary>
    /// <param name="sampleRate">The sample rate for the audio playback (default is 44100).</param>
    /// <param name="channelCount">The number of audio channels (default is 2).</param>
    public SoundPlayer(int sampleRate = 44100, int channelCount = 2) {
        outputDevice = new WaveOutEvent();
        mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
        mixer.ReadFully = true;
        outputDevice.Init(mixer);
        outputDevice.Play();
    }

    /// <summary>
    /// Plays a sound from the specified file.
    /// </summary>
    /// <param name="fileName">The path to the audio file to play.</param>
    public void PlaySound(string fileName) {
        var input = new AudioFileReader(fileName);
        AddMixerInput(new AutoDisposeFileReader(input));
    }

    /// <summary>
    /// Plays a cached sound.
    /// </summary>
    /// <param name="sound">The cached sound to play.</param>
    public void PlaySound(CachedSound sound) {
        AddMixerInput(new CachedSoundSampleProvider(sound));
    }

    /// <summary>
    /// Stops all currently playing sounds.
    /// </summary>
    public void StopSound() {
        if (outputDevice.PlaybackState == PlaybackState.Playing) {
            outputDevice.Stop();
        }
    }

    /// <summary>
    /// Releases all resources used by the <see cref="SoundPlayer"/> instance.
    /// </summary>
    public void Dispose() {
        outputDevice.Dispose();
    }

    /// <summary>
    /// Converts an input sample provider to match the channel count of the mixer.
    /// </summary>
    /// <param name="input">The input sample provider.</param>
    /// <returns>A sample provider with the correct channel count.</returns>
    /// <exception cref="NotImplementedException">
    /// Thrown if the channel count conversion is not implemented.
    /// </exception>
    private ISampleProvider ConvertToRightChannelCount(ISampleProvider input) {
        if (input.WaveFormat.Channels == mixer.WaveFormat.Channels) {
            return input;
        }
        if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2) {
            return new MonoToStereoSampleProvider(input);
        }
        throw new NotImplementedException("Not yet implemented this channel count conversion");
    }

    /// <summary>
    /// Adds an input sample provider to the mixer.
    /// </summary>
    /// <param name="input">The input sample provider to add.</param>
    private void AddMixerInput(ISampleProvider input) {
        mixer.AddMixerInput(ConvertToRightChannelCount(input));
    }

    /// <summary>
    /// A singleton instance of the <see cref="SoundPlayer"/> class.
    /// </summary>
    public static readonly SoundPlayer Instance = new SoundPlayer(44100, 2);
}
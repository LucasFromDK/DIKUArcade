namespace DIKUArcade.Sound;

using System;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
public class SoundPlayer : IDisposable {
    private readonly IWavePlayer outputDevice;
    private readonly MixingSampleProvider mixer;

    public SoundPlayer(int sampleRate = 44100, int channelCount = 2) {
        outputDevice = new WaveOutEvent();
        mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
        mixer.ReadFully = true;
        outputDevice.Init(mixer);
        outputDevice.Play();
    }

    public void PlaySound(string fileName) {
        var input = new AudioFileReader(fileName);
        AddMixerInput(new AutoDisposeFileReader(input));
    }

    private ISampleProvider ConvertToRightChannelCount(ISampleProvider input) {
        if (input.WaveFormat.Channels == mixer.WaveFormat.Channels) {
            return input;
        }
        if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2) {
            return new MonoToStereoSampleProvider(input);
        }
        throw new NotImplementedException("Not yet implemented this channel count conversion");
    }

    public void PlaySound(CachedSound sound) {
        AddMixerInput(new CachedSoundSampleProvider(sound));
    }

    public void StopSound() {
        if (outputDevice.PlaybackState == PlaybackState.Playing) {
            outputDevice.Stop();
        }
    }

    private void AddMixerInput(ISampleProvider input) {
        mixer.AddMixerInput(ConvertToRightChannelCount(input));
    }

    public void Dispose() {
        outputDevice.Dispose();
    }

    public static readonly SoundPlayer Instance = new SoundPlayer(44100, 2);
}
namespace DIKUArcade.Sound;
using System;
using System.Media;

public class CustomSoundPlayer {
    private SoundPlayer player;

    /// <summary>
    /// Constructor for CustomSoundPlayer.
    /// </summary>
    /// <param name="soundFilePath">
    /// The path to the sound file that should be played.
    /// </param>
    public CustomSoundPlayer(string soundFilePath) {
        player = new System.Media.SoundPlayer(soundFilePath);
    }

    /// <summary>
    /// Play the sound once.
    /// </summary>
    public void Play() {
        player.Play();
    }

    /// <summary>
    /// Play the sound in a loop.
    /// </summary>
    public void PlayLooping() {
        player.PlayLooping();
    }

    /// <summary>
    /// Stop the sound.
    /// </summary>
    public void Stop() {
        player.Stop();
    }
}
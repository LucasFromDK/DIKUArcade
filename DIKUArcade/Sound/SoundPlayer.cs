namespace DIKUArcade.Sound;
using System;
using System.Media;

public class CustomSoundPlayer {
    private SoundPlayer player;

    public CustomSoundPlayer(string soundFilePath) {
        // Create a System.Media.SoundPlayer
        player = new System.Media.SoundPlayer(soundFilePath);
    }

    public void Play() {
        player.Play();
    }

    public void PlayLooping() {
        player.PlayLooping();
    }

    public void Stop() {
        player.Stop();
    }
}
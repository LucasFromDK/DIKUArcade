namespace DIKUArcade.Sound;

using NAudio.Wave;

/// <summary>
/// A wrapper for <see cref="AudioFileReader"/> that automatically disposes of the reader
/// when the end of the audio file is reached.
/// </summary>
public class AutoDisposeFileReader : ISampleProvider {
    private readonly AudioFileReader reader;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoDisposeFileReader"/> class.
    /// </summary>
    /// <param name="reader">The <see cref="AudioFileReader"/> to wrap and automatically dispose.</param>
    public AutoDisposeFileReader(AudioFileReader reader) {
        this.reader = reader;
        this.WaveFormat = reader.WaveFormat;
    }

    /// <summary>
    /// Reads audio samples into the provided buffer.
    /// Automatically disposes of the reader when the end of the file is reached.
    /// </summary>
    /// <param name="buffer">The buffer to fill with audio samples.</param>
    /// <param name="offset">The offset in the buffer to start writing samples.</param>
    /// <param name="count">The number of samples to read.</param>
    /// <returns>The number of samples actually read.</returns>
    public int Read(float[] buffer, int offset, int count) {
        if (isDisposed)
            return 0;
        int read = reader.Read(buffer, offset, count);
        if (read == 0) {
            reader.Dispose();
            isDisposed = true;
        }
        return read;
    }

    /// <summary>
    /// Gets the wave format of the audio file.
    /// </summary>
    public WaveFormat WaveFormat {
        get; private set;
    }
}
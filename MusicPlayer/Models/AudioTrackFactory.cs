using System.IO;
using System.Threading.Tasks;
using SoundFlow.Metadata;
using SoundFlow.Metadata.Models;

namespace MusicPlayer.Models;

public static class AudioTrackFactory
{
    /// <summary>
    /// Creates an <see cref="AudioTrack"/> from a file source.
    /// </summary>
    /// <param name="filePath">The file to create the <see cref="AudioTrack"/> from.</param>
    /// <returns>An <see cref="AudioTrack"/> if the file was read successfully, or null.</returns>
    public static async Task<AudioTrack?> CreateFromFileAsync(string filePath)
    {
        if (!Path.Exists(filePath))
        {
            return null;
        }

        var result = await SoundMetadataReader.ReadAsync(filePath, new ReadOptions
        {
            DurationAccuracy = DurationAccuracy.AccurateScan
        }).ConfigureAwait(false);
        if (!result.IsSuccess)
        {
            return null;
        }
        
        var meta = result.Value!;
        var trackMeta = new TrackMeta(
                meta.Tags?.Title ?? "", 
                meta.Tags?.Artist ?? "", 
                meta.Tags?.Album ?? "",
                meta.Tags?.Genre ?? "",
                meta.Tags?.Year,
                meta.Tags?.TrackNumber,
                meta.Duration);

        return new AudioTrack(filePath, TrackSource.File, trackMeta);
    }
}
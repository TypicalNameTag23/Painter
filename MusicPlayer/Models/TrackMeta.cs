namespace MusicPlayer.Models;

public record TrackMeta(string Title, string Artist, string Album, string Genre, uint? ReleaseYear, uint? TrackNumber, TimeSpan Duration);
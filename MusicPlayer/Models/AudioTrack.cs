using System;

namespace MusicPlayer.Models;

public record AudioTrack(string Path, TrackSource TrackSource, TrackMeta TrackMeta) : IComparable
{
    private const int Precedes = -1;
    private const int Same = 0;
    private const int Follows = 1;
    
    public int CompareTo(object? obj)
    {
        if (obj is not AudioTrack other || TrackMeta.TrackNumber < other.TrackMeta.TrackNumber)
        {
            return Precedes;
        }

        return TrackMeta.TrackNumber > other.TrackMeta.TrackNumber ? 
            Follows : Same;
    }
}
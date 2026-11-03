namespace MusicPlayer.Player;

/// <summary>
/// Determines how the player plays its tracks 
/// </summary>
public enum LoopType
{
    /// <summary>
    /// Loops back to first track after reaching the end of the track list
    /// </summary>
    LoopQueue,
    
    /// <summary>
    /// Loops a single track
    /// </summary>
    LoopTrack,
    
    /// <summary>
    /// Stops at the end of the track list
    /// </summary>
    Disabled
}
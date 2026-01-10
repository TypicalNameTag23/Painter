using MusicPlayer.Events;
using MusicPlayer.Models;

namespace MusicPlayer.Player;

/// <summary>
/// 
/// </summary>
public interface IAudioPlayer : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets a list of the <see cref="AudioTrack"/>s that have been added to this player
    /// </summary>
    public IReadOnlyList<AudioTrack> Tracks { get; }
    
    /// <summary>
    /// Gets the currently selected <see cref="AudioTrack"/>
    /// </summary>
    public AudioTrack? CurrentTrack { get; }
    
    /// <summary>
    /// Gets the index of the currently selected track in the list of <see cref="Tracks"/>
    /// </summary>
    public int CurrentTrackIndex { get; protected set; }
    
    /// <summary>
    /// Gets the current <see cref="PlayerState"/>
    /// </summary>
    public PlayerState PlayerState { get; protected set; }
    
    /// <summary>
    /// Gets or sets the current <see cref="LoopType"/>
    /// </summary>
    public LoopType LoopType { get; set; }
    
    /// <summary>
    /// Gets the <see cref="CurrentTrack"/>'s track length
    /// </summary>
    public TimeSpan TrackLength { get; }
    
    /// <summary>
    /// Gets the amount of time that the <see cref="CurrentTrack"/> has been played for 
    /// </summary>
    public TimeSpan TrackTime { get; }

    event PlaybackTimeChangedEvent PlaybackTimeChangedEvent;
    event TrackChangedEvent TrackChangedEvent; 
    event LoopTypeChangedEvent? LoopTypeChangedEvent;
    event PlayerStateChangedEvent? PlayerStateChangedEvent;
    
    /// <summary>
    /// Queues an <see cref="AudioTrack"/>
    /// </summary>
    /// <param name="track"><see cref="AudioTrack"/> to enqueue</param>
    public void Queue(AudioTrack track);
    
    /// <summary>
    /// Queues a list of <see cref="AudioTrack"/>s
    /// </summary>
    /// <param name="tracks">The list of <see cref="AudioTrack"/>s to enqueue</param>
    public void Queue(List<AudioTrack> tracks);

    /// <summary>
    /// Places an <see cref="AudioTrack"/> at the start of the queue
    /// </summary>
    /// <param name="track"><see cref="AudioTrack"/> to enqueue</param>
    public void QueueNext(AudioTrack track);

    /// <summary>
    /// Dequeues a track at the specified index
    /// </summary>
    /// <param name="trackIndex">The index of the <see cref="AudioTrack"/> in the <see cref="Tracks"/> list to dequeue</param>
    public void Dequeue(int trackIndex);

    /// <summary>
    /// Dequeues the first matching <see cref="AudioTrack"/>
    /// </summary>
    /// <param name="track">The <see cref="AudioTrack"/> to dequeue</param>
    public void Dequeue(AudioTrack track);

    /// <summary>
    /// Clears the <see cref="Tracks"/> list
    /// </summary>
    public void ClearQueue();

    /// <summary>
    /// Plays the first <see cref="AudioTrack"/> in the <see cref="Tracks"/> queue if the <see cref="PlayerState"/> is <see cref="PlayerState.Stopped"/>, resumes the <see cref="CurrentTrack"/> if the <see cref="PlayerState"/> is <see cref="PlayerState.Paused"/>
    /// </summary>
    /// <param name="force">Force the player to play the current track, even if <see cref="PlayerState"/> is <see cref="PlayerState.Playing"/></param>
    public void Play(bool force = false);
    
    /// <summary>
    /// Pauses the player
    /// </summary>
    public void Pause();
    
    /// <summary>
    /// Restarts the currently playing track
    /// </summary>
    public void Replay();
    
    /// <summary>
    /// Stops the player
    /// </summary>
    public void Stop();
    
    /// <summary>
    /// Seeks to a specified time in the current track
    /// </summary>
    public void SeekTo(TimeSpan time);
    
    /// <summary>
    /// Seeks a specified amount of time
    /// </summary>
    public void Seek(TimeSpan time);
    
    /// <summary>
    /// Plays the next track
    /// </summary>
    public void NextTrack();
    
    /// <summary>
    /// Plays the previous track
    /// </summary>
    public void PreviousTrack();
    
    /// <summary>
    /// Skips to the next track specified by the <see cref="LoopType"/>
    /// </summary>
    public void Skip();
    
    /// <summary>
    /// Skips to the previous track specified by the <see cref="LoopType"/>
    /// </summary>
    public void SkipBack();
}
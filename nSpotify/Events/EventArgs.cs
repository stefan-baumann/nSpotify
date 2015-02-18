using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nSpotify
{
    /// <summary>
    /// Provides information when the <see cref="Track"/> Spotify plays changed
    /// </summary>
    public class TrackChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackChangedEventArgs"/> class.
        /// </summary>
        /// <param name="lastTrack">The last track.</param>
        /// <param name="currentTrack">The current track.</param>
        internal TrackChangedEventArgs(Track lastTrack, Track currentTrack)
        {
            this.LastTrack = lastTrack;
            this.CurrentTrack = currentTrack;
        }

        /// <summary>
        /// Returns the last track
        /// </summary>
        /// <value>
        /// The last track.
        /// </value>
        public Track LastTrack { get; protected set; }

        /// <summary>
        /// Returns the current track.
        /// </summary>
        /// <value>
        /// The current track.
        /// </value>
        public Track CurrentTrack { get; protected set; }
    }

    /// <summary>
    /// Provides information when the Spotify volume changed
    /// </summary>
    public class VolumeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="lastVolume">The last volume.</param>
        /// <param name="currentVolume">The current volume.</param>
        internal VolumeChangedEventArgs(double lastVolume, double currentVolume)
        {
            this.LastVolume = lastVolume;
            this.CurrentVolume = currentVolume;
        }

        /// <summary>
        /// Returns the last volume.
        /// </summary>
        /// <value>
        /// The last volume.
        /// </value>
        public double LastVolume { get; protected set; }

        /// <summary>
        /// Returns the current volume.
        /// </summary>
        /// <value>
        /// The current volume.
        /// </value>
        public double CurrentVolume { get; protected set; }
    }

    /// <summary>
    /// Provides information when the Spotify play state changed
    /// </summary>
    public class PlayStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="playing">Indicates whether Spotify is currently playing.</param>
        internal PlayStateChangedEventArgs(bool playing)
        {
            this.Playing = playing;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PlayStateChangedEventArgs"/> is playing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if playing; otherwise - when paused, <c>false</c>.
        /// </value>
        public bool Playing { get; protected set; }
    }

    /// <summary>
    /// Provides information when the Spotify status data was updated
    /// </summary>
    public class DataUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="lastStatus">The last status.</param>
        /// <param name="currentStatus">The current status.</param>
        internal DataUpdatedEventArgs(Status lastStatus, Status currentStatus)
        {
            this.CurrentStatus = currentStatus;
            this.LastStatus = lastStatus;
        }

        /// <summary>
        /// Returns the current status.
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        public Status CurrentStatus { get; protected set; }

        /// <summary>
        /// Returns the last status.
        /// </summary>
        /// <value>
        /// The last status.
        /// </value>
        public Status LastStatus { get; protected set; }
    }
}

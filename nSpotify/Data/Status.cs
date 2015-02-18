using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nSpotify
{
    /// <summary>
    /// Stores the status information fetched from the SpotifyWebHelper.exe
    /// </summary>
    /// 
    [JsonObject(MemberSerialization.OptIn)]
    public class Status
    {
        /// <summary>
        /// Returns the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonProperty("version")]
        internal string Version { get; set; }

        /// <summary>
        /// Returns the version of the Spotify Client.
        /// </summary>
        /// <value>
        /// The client version.
        /// </value>
        [JsonProperty("client_version")]
        internal string ClientVersion { get; set; }

        /// <summary>
        /// Returns a value indicating whether Spotify is playing a track.
        /// </summary>
        /// <value>
        ///   <c>true</c> if playing a track; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("playing")]
        public bool Playing { get; protected set; }

        /// <summary>
        /// Returns a value indicating whether Spotify is playing in shuffle mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if Spotify is playing in shuffle mode; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("shuffle")]
        public bool Shuffle { get; protected set; }

        /// <summary>
        /// Returns a value indicating whether Spotify is playing in repeat mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if if Spotify is playing in repeat mode; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("repeat")]
        public bool Repeat { get; protected set; }

        /// <summary>
        /// Returns a value indicating whether Spotify has playing enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if playing is enabled; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("play_enabled")]
        public bool PlayEnabled { get; protected set; }

        /// <summary>
        /// Returns a value indicating whether it is possible to go back to the previous track.
        /// This is normally disabled if an ad is playing
        /// </summary>
        /// <value>
        ///   <c>true</c> if is possible to go back to the previous track; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("prev_enabled")]
        public bool PrevEnabled { get; protected set; }

        /// <summary>
        /// Returns a value indicating whether it is possible to go to the next track.
        /// This is normally disabled if an ad is playing
        /// </summary>
        /// <value>
        ///   <c>true</c> if is possible to go to the next track; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("next_enabled")]
        public bool NextEnabled { get; protected set; }

        /// <summary>
        /// Gets or sets the track.
        /// </summary>
        /// <value>
        /// The track.
        /// </value>
        [JsonProperty("track")]
        public Track Track { get; protected set; }

        /// <summary>
        /// Returns the playing position of Spotify.
        /// </summary>
        /// <value>
        /// The playing position.
        /// </value>
        [JsonProperty("playing_position")]
        [JsonConverter(typeof(SpotifyTimeSpanConverter))]
        public TimeSpan PlayingPosition { get; protected set; }

        /// <summary>
        /// Returns the server time.
        /// </summary>
        /// <value>
        /// The server time.
        /// </value>
        [JsonProperty("server_time")]
        [JsonConverter(typeof(SpotifyDateTimeConverter))]
        internal DateTime ServerTime { get; set; }

        /// <summary>
        /// Returns the volume Spotify is playing tracks with
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        [JsonProperty("volume")]
        public double Volume { get; protected set; }

        /// <summary>
        /// Returns a value indicating whether Spotify is in Online Mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if online; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("online")]
        public bool Online { get; protected set; }

        //Maybe implement open_graph_state?

        /// <summary>
        /// Returns a value indicating whether this <see cref="Status"/> is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("running")]
        public bool Running { get; protected set; }

        /// <summary>
        /// If an error occurs, this field will contain additional information
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        [JsonProperty("error")]
        internal Error Error { get; set; }
    }
}

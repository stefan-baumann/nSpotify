using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace nSpotify
{
    /// <summary>
    /// Stores information of a Spotify track
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Track
    {
        /// <summary>
        /// Returns the name of the track.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.TrackResource?.Name;
            }
        }

        /// <summary>
        /// Returns the <see cref="Resource"/> of the track.
        /// </summary>
        /// <value>
        /// The track resource.
        /// </value>
        [JsonProperty("track_resource")]
        public Resource TrackResource { get; protected set; }

        /// <summary>
        /// Returns the name of the artist(s) who created the track.
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        public string Artist
        {
            get
            {
                return this.ArtistResource?.Name;
            }
        }

        /// <summary>
        /// Returns the <see cref="Resource"/> of the artist(s) who created the track.
        /// </summary>
        /// <value>
        /// The artist resource.
        /// </value>
        [JsonProperty("artist_resource")]
        public Resource ArtistResource { get; protected set; }

        /// <summary>
        /// Returns the name of the album the track is belongs to.
        /// </summary>
        /// <value>
        /// The album.
        /// </value>
        public string Album
        {
            get
            {
                return this.AlbumResource?.Name;
            }
        }

        /// <summary>
        /// Returns the <see cref="Resource"/> of the album the track is belongs to.
        /// </summary>
        /// <value>
        /// The album resource.
        /// </value>
        [JsonProperty("album_resource")]
        public Resource AlbumResource { get; protected set; }

        /// <summary>
        /// Returns the length of the track.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        [JsonProperty("length")]
        [JsonConverter(typeof(SpotifyTimeSpanConverter))]
        public TimeSpan Length { get; protected set; }

        /// <summary>
        /// Returns the track type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty("track_type")]
        public string Type { get; protected set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object to compare with the left one.</param>
        /// <returns>
        ///   <c>true</c> if both tracks are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(Track left, Track right)
        {
            return (object)right != null && left.TrackResource == right.TrackResource && left.ArtistResource == right.ArtistResource && left.AlbumResource == right.AlbumResource && left.Length == right.Length && left.Type == right.Type;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right object to compare with the left one.</param>
        /// <returns>
        ///   <c>true</c> if both tracks are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(Track left, Track right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.TrackResource.GetHashCode() ^ this.ArtistResource.GetHashCode() ^ this.AlbumResource.GetHashCode() ^ this.Length.GetHashCode() ^ this.Type.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{{Name = \"{0}\"; Artist = \"{1}\"; Album = \"{2}\"; Length = \"{3}\"; Type = \"{4}\"}}", this.Name, this.Artist, this.Album, this.Length, this.Type);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == typeof(Track) && this == (Track)obj;
        }

        /// <summary>
        /// Downloads the album art.
        /// </summary>
        /// <param name="size">The size of the album art.</param>
        /// <returns>The album art.</returns>
        public Image DownloadAlbumArt(AlbumArtSize size)
        {
            using (WebClient client = new WebClient() { Proxy = null })
            {
                string source = client.DownloadString(string.Format("http://open.spotify.com/album/{0}", this.AlbumResource?.InternalUri?.AbsolutePath?.Split(':')[1])).Replace("\t", string.Empty);
                foreach (string line in source.Split('\r', '\n'))
                {
                    if (line.StartsWith("<meta property=\"og:image\""))
                    {
                        byte[] image = client.DownloadData(line.Split('"')[3].Replace("image", ((int)size).ToString()));
                        using (MemoryStream stream = new MemoryStream(image))
                        {
                            return Image.FromStream(stream);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// An enum used for specifying the size of the cover to download
        /// </summary>
        public enum AlbumArtSize : int
        {
            Size160 = 160,
            Size320 = 320,
            Size640 = 640
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nSpotify
{
    /// <summary>
    /// Stores information of a Spotify album, artist or track.
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// Returns the name of the album, artist or track.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; protected set; }

        /// <summary>
        /// Returns the internally used <see cref="System.Uri"/> of the album, artist or track.
        /// </summary>
        /// <value>
        /// The internal URI.
        /// </value>
        [JsonProperty("uri")]
        public Uri InternalUri { get; protected set; }

        //Implement this one (located in resource.location.og instead of resource.location - who does this shit? -_-)
        //
        ///// <summary>
        ///// Returns the webplayer <see cref="System.Uri"/> of the album, artist or track.
        ///// </summary>
        ///// <value>
        ///// The webplayer URI.
        ///// </value>
        //[JsonProperty("location")]
        //public Uri WebplayerUri { get; protected set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object to compare with the left one.</param>
        /// <returns>
        ///   <c>true</c> if both tracks are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(Resource left, Resource right)
        {
            return left?.Name == right?.Name && left?.InternalUri == right?.InternalUri;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right object to compare with the left one.</param>
        /// <returns>
        ///   <c>true</c> if both tracks are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(Resource left, Resource right)
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
            return this.Name.GetHashCode() ^ this.InternalUri.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{{Name = \"{0}\"; InternalUri = \"{1}\"}}", this.Name, this.InternalUri);
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
            return obj != null && obj.GetType() == typeof(Resource) && this == (Resource)obj;
        }
    }
}

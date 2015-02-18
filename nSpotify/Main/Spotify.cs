using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nSpotify
{
    /// <summary>
    /// Provides methods to access data from Spotify.
    /// </summary>
    public class Spotify
    {
        private static object providerInstanceSyncRoot = new object();
        private static DataProvider providerInstance = null;

        /// <summary>
        /// Provides the main <see cref="DataProvider"/> instance. If it isn't initialized yet, it will be after the first call to this property.
        /// </summary>
        /// <value>
        /// The main <see cref="DataProvider"/> instance.
        /// </value>
        public static DataProvider DataProviderInstance
        {
            get
            {
                if (Spotify.providerInstance == null)
                {
                    lock (providerInstanceSyncRoot)
                    {
                        if (Spotify.providerInstance == null)
                        {
                            Spotify.providerInstance = new DataProvider();
                        }
                    }
                }

                return Spotify.providerInstance;
            }
        }

        /// <summary>
        /// Returns a value indicating whether Spotify is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if Spotify is running; otherwise, <c>false</c>.
        /// </value>
        public static bool SpotifyRunning
        {
            get
            {
                return (Process.GetProcessesByName("Spotify").Length > 0);
            }
        }

        /// <summary>
        /// Starts Spotify if it isn't already running
        /// </summary>
        /// <param name="launchIfRunning">if set to <c>true</c> the method will try to launch Spotify even if it is already running.</param>
        public static void StartSpotify(bool launchIfRunning = false)
        {
            if (launchIfRunning || !Spotify.SpotifyRunning)
            {
                Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Spotify\SpotifyLauncher.exe"));
            }
        }

        /// <summary>
        /// Returns a value indicating whether the Spotify Web Helper is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the Spotify Web Helper is running; otherwise, <c>false</c>.
        /// </value>
        public static bool SpotifyWebHelperRunning
        {
            get
            {
                return (Process.GetProcessesByName("SpotifyWebHelper").Length > 0);
            }
        }

        /// <summary>
        /// Starts the Spotify Web Helper if it isn't already running
        /// </summary>
        /// <param name="launchIfRunning">if set to <c>true</c> the method will try to launch the Spotify Web Helper even if it is already running.</param>
        public static void StartSpotifyWebHelper(bool launchIfRunning = false)
        {
            if (launchIfRunning || !Spotify.SpotifyRunning)
            {
                Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Spotify\Data\SpotifyWebHelper.exe"));
            }
        }

        //Todo: Implement methods which take a Track or a Resource
        /// <summary>
        /// Sends a play request to Spotify.
        /// </summary>
        /// <param name="trackUri">If specified, the method will queue the track and then play it.</param>
        public static void SendPlayRequest(string trackUri = "")
        {
            Spotify.DataProviderInstance.QueryRequest(string.IsNullOrWhiteSpace(trackUri) ? "remote/pause.json?pause=false" : "remote/play.json?uri=" + trackUri);
        }

        /// <summary>
        /// Sends a pause request to Spotify.
        /// </summary>
        public static void SendPauseRequest()
        {
            Spotify.DataProviderInstance.QueryRequest("remote/pause.json?pause=true");
        }

        /// <summary>
        /// Sends a the queue request to Spotify.
        /// </summary>
        /// <param name="trackUri">The track the method should queue.</param>
        public static void SendQueueRequest(string trackUri)
        {
            Spotify.DataProviderInstance.QueryRequest("remote/play.json?uri=" + trackUri + "?action=queue");
        }
    }
}

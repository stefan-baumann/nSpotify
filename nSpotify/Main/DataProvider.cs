using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace nSpotify
{
    /// <summary>
    /// Provides data fetched from the SpotifyWebHelper.exe
    /// </summary>
    public class DataProvider
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvider"/> class.
        /// If possible though, you should use the main instance (<see cref="DataProvider.MainInstance"/>) - using more than one instance at once may cause problems.
        /// </summary>
        public DataProvider()
            : this("127.0.0.1")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvider"/> class.
        /// If possible though, you should use the main instance (<see cref="DataProvider.MainInstance"/>) - using more than one instance at once may cause problems.
        /// </summary>
        /// <param name="host">The host. Use 127.0.0.1 (localhost) or whatever you like.</param>
        public DataProvider(string host)
            : this(host, DataProvider.GetOAuthToken())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvider"/> class.
        /// If possible though, you should use the main instance (<see cref="DataProvider.MainInstance"/>) - using more than one instance at once may cause problems.
        /// </summary>
        /// <param name="host">The host. Use 127.0.0.1 (localhost) or whatever you like.</param>
        /// <param name="oAuth">The OAuth token (you can get one from <see cref="DataProvider.GetOAuthToken"/>).</param>
        public DataProvider(string host, string oAuth)
        {
            //Initialize the WebClient used to request all the information. Setting the Origin and Referer is needed for Spotify to allow us to use their service.
            //Basically we emulate the code used in the Spotify embedded player.
            this.client = new WebClient();
            this.client.Proxy = null;
            this.client.Headers.Add("Origin", "https://embed.spotify.com");
            this.client.Headers.Add("Referer", DataProvider.embeddedTrack);
            this.client.Encoding = Encoding.UTF8;

            this.CurrentStatus = null;

            this.host = host;
            this.oAuth = oAuth;
            this.cfid = this.GetCfid();
        }

        protected WebClient client { get; private set; }
        protected string host { get; private set; }
        protected string oAuth { get; private set; }
        protected string cfid { get; private set; }

        /// <summary>
        /// The embedded track used for getting the OAuth token and to emulate an embedded player.
        /// If there is a problem when fetching the data, try replacing this url with another one. You can get it by right-clicking on any song and pressing "Copy Spotify Uri". Then put the part after "spotify:track:" in place of the current track id.
        /// </summary>
        protected const string embeddedTrack = @"http://embed.spotify.com/track/7pqgMEKsDMOHUdFQ7n0N9K";

        //Todo: Test if you can use http://open.spotify.com/token instead of the shitty embedded player workaround
        //      Also provide better Error messages
        /// <summary>
        /// Retrieves a OAuth token from the spotify embedded player.
        /// This method is mainly used internally during the initialisation of a <see cref="DataProvider"/>.
        /// </summary>
        /// <returns>A OAuth token used for the initialisation of a <see cref="DataProvider"/>.</returns>
        /// <exception cref="System.Exception">Could not find the OAuth token.</exception>
        public static string GetOAuthToken()
        {
            using (WebClient downloadClient = new WebClient())
            {
                //We need to specify a UserAgent header because Spotify blocked access to WebClients without a valid UserAgent
                downloadClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                //string webSource = downloadClient.DownloadString(DataProvider.embeddedTrack).Replace(" ", string.Empty).Replace("\t", string.Empty);
                //foreach(string line in webSource.Split('\n', '\r'))
                //{
                //    //The line we are looking for looks like this (without the "): "tokenData = '<token>'," where <token> represents where the OAuth token
                //    if (line.StartsWith("tokenData"))
                //    {
                //        return line.Split('\'')[1];
                //    }
                //}

                string json = downloadClient.DownloadString("http://open.spotify.com/token");
                dynamic deserialized = JsonConvert.DeserializeObject(json);
                if (deserialized.t != null)
                {
                    return deserialized.t.ToString();
                }

                throw new Exception("Could not find the OAuth token.");
            }
        }

        /// <summary>
        /// Retrieves a Cfid by calling <value>simplecsrf/token.json</value>.
        /// </summary>
        /// <returns>A valid Cfid.</returns>
        /// <exception cref="System.Exception">
        /// Cfid couldn't be loaded: The query response was empty.
        /// or
        /// Cfid could not be loaded: The query response contained multiple or no Cfids.
        /// or
        /// Cfid could not be loaded, a internal error of type [...] occured: [...].
        /// or
        /// Cfid could not be loaded: The token field was empty.
        /// </exception>
        public string GetCfid()
        {
            //Get the response
            string response = this.QueryRequest("simplecsrf/token.json", false, false).Replace("\\", "");
            List<Cfid> cfidList = JsonConvert.DeserializeObject<List<Cfid>>(response);

            //Throw Exceptions if needed or return the cfid
            if (cfidList == null)
                throw new Exception("Cfid couldn't be loaded: The query response was empty.");
            else if (cfidList.Count != 1)
                throw new Exception("Cfid could not be loaded: The query response contained multiple or no Cfids.");
            else if (cfidList[0].Error != null)
                throw new Exception(string.Format("Cfid could not be loaded, a internal error of type \"{0}\" occured: {1}", cfidList[0].Error.Type, cfidList[0].Error.Message));
            else if (string.IsNullOrWhiteSpace(cfidList[0].Token))
                throw new Exception("Cfid could not be loaded: The token field was empty.");
            else
                return cfidList[0].Token;
        }

        //Todo: Add Exceptions
        /// <summary>
        /// Queries a request to the Spotify Api.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="addOAuth">if set to <c>true</c>, the method will add the OAuth token to the request. This is necessary for most requests.</param>
        /// <param name="addCfid">if set to <c>true</c>, the method will add the Cfid to the request. This is necessary for most requests.</param>
        /// <returns>The response.</returns>
        protected internal string QueryRequest(string request, bool addOAuth = true, bool addCfid = true)
        {
            int timestamp = (int)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            string parameters = string.Format("{0}&ref=&cors=&_={1}{2}{3}",
                request.Contains('?') ? string.Empty : "?",
                timestamp,
                addOAuth ? "&oauth=" + this.oAuth : string.Empty,
                addCfid ? "&csrf=" + this.cfid : string.Empty);
            string url = string.Format("https://nspotify.spotilocal.com:4371/{0}{1}", request, parameters);
            string response = this.client.DownloadString(url);
            return string.Format("[ {0} ]", response);
        }

        /// <summary>
        /// Returns the current status.
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        public Status CurrentStatus { get; protected set; }

        /// <summary>
        /// Updates the current status and stores it in <see cref="DataProvider.CurrentStatus"/>.
        /// </summary>
        /// <returns>The current status.</returns>
        /// <exception cref="System.Exception">
        /// Status could not be loaded: The query response was empty
        /// or
        /// Status couldn't be loaded: The query response was empty.
        /// or
        /// Status could not be loaded: The query response contained multiple or no Statuses.
        /// or
        /// Status could not be loaded, a internal error of type [...] occured: [...].
        /// </exception>
        public Status UpdateStatus()
        {
            string response = this.QueryRequest("remote/status.json");
            if (string.IsNullOrWhiteSpace(response))
                throw new Exception("Status could not be loaded: The query response was empty");
            List<Status> statusList = JsonConvert.DeserializeObject<List<Status>>(response);

            if (statusList == null)
                throw new Exception("Status couldn't be loaded: The query response was empty.");
            else if (statusList.Count != 1)
                throw new Exception("Status could not be loaded: The query response contained multiple or no Statuses.");
            else if (statusList[0].Error != null)
                throw new Exception(string.Format("Status could not be loaded, a internal error of type \"{0}\" occured: {1}", statusList[0].Error.Type, statusList[0].Error.Message));
            else
                this.CurrentStatus = statusList[0];
            return this.CurrentStatus;
        }
    }
}
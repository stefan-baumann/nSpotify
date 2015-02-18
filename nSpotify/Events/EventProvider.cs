using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace nSpotify
{
    /// <summary>
    /// Provides managed events for Spotify.
    /// </summary>
    public class EventProvider : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventProvider"/> class.
        /// It will use the main <see cref="DataProvider"/> instance.
        /// </summary>
        public EventProvider()
            : this(Spotify.DataProviderInstance)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventProvider"/> class.
        /// </summary>
        /// <param name="dataProvider">The data provider to get data from.</param>
        public EventProvider(DataProvider dataProvider)
            : this(dataProvider, 50)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventProvider"/> class.
        /// </summary>
        /// <param name="dataProvider">The data provider to get data from.</param>
        /// <param name="interval">The interval in milliseconds the timer should fetch the data.</param>
        public EventProvider(DataProvider dataProvider, double interval)
            : this(dataProvider, interval, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventProvider"/> class.
        /// </summary>
        /// <param name="dataProvider">The data provider to get data from</param>
        /// <param name="interval">The interval in milliseconds the timer should fetch the data.</param>
        /// <param name="synchronizingObject">The synchronizing object. Specify your Form or Control here so that you don't have to invoke when handling events.</param>
        public EventProvider(DataProvider dataProvider, double interval, ISynchronizeInvoke synchronizingObject)
            : this(dataProvider, interval, synchronizingObject, false)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventProvider"/> class.
        /// </summary>
        /// <param name="dataProvider">The data provider to get data from</param>
        /// <param name="interval">The interval in milliseconds the timer should fetch the data.</param>
        /// <param name="synchronizingObject">The synchronizing object. Specify your Form or Control here so that you don't have to invoke when handling events.</param>
        /// <param name="enabled">if set to <c>true</c> this class will immediately start fetching data from Spotify.</param>
        public EventProvider(DataProvider dataProvider, double interval, ISynchronizeInvoke synchronizingObject, bool enabled)
        {
            this.DataProvider = dataProvider;
            this.timer = new Timer(interval) { AutoReset = false };
            this.timer.Elapsed += (s, e) => this.UpdateData();
            this.EventSynchronizingObject = synchronizingObject;
            this.Enabled = enabled;
        }

        /// <summary>
        /// Starts the timer to start fetching data from Spotify.
        /// </summary>
        public void Start()
        {
            this.timer.Start();
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            this.timer.Stop();
        }

        /// <summary>
        /// Gets or Sets a value indicating whether this <see cref="EventProvider"/> is fetching data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled
        {
            get
            {
                return this.timer.Enabled;
            }
            set
            {
                this.timer.Enabled = value;
            }
        }

        /// <summary>
        /// Updates the data from Spotify and raises events if necessary.
        /// </summary>
        protected void UpdateData()
        {
            Status lastStatus = this.DataProvider.CurrentStatus;
            Status status = this.DataProvider.UpdateStatus();

            //We use a anonymous method so we don't need a method outside of the void
            Action<Status, Status> Updater = (Status cur, Status last) =>
            {
                OnDataUpdated(new DataUpdatedEventArgs(cur, last));
                if (last != null && cur != null)
                {
                    if (cur.Track != last.Track) OnTrackChanged(new TrackChangedEventArgs(last.Track, cur.Track));
                    if (cur.Volume != last.Volume) OnVolumeChanged(new VolumeChangedEventArgs(last.Volume, cur.Volume));
                    if (cur.Playing != last.Playing) OnPlayStateChanged(new PlayStateChangedEventArgs(cur.Playing));
                }
            };

            //Test if we need to invoke
            if (this.EventSynchronizingObject != null && this.EventSynchronizingObject.InvokeRequired)
            {
                this.EventSynchronizingObject.BeginInvoke(Updater, new object[] { status, lastStatus });
            }
            else
            {
                Updater(status, lastStatus);
            }

            //Restart the cycle
            this.timer.Start();
        }

        /// <summary>
        /// The internally used data provider.
        /// </summary>
        /// <value>
        /// The data provider.
        /// </value>
        public DataProvider DataProvider { get; protected set; }

        /// <summary>
        /// Returns the current status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status
        {
            get
            {
                return this.DataProvider.CurrentStatus;
            }
        }

        /// <summary>
        /// If specified, all raised events will be synchronized to this object
        /// </summary>
        /// <value>
        /// The synchronizing object.
        /// </value>
        public ISynchronizeInvoke EventSynchronizingObject { get; set; }

        /// <summary>
        /// The timer internally used to refresh the data fetched from Spotify
        /// </summary>
        /// <value>
        /// The timer.
        /// </value>
        protected Timer timer { get; private set; }

        /// <summary>
        /// Gets fired when the track Spotify is currently playing changed
        /// </summary>
        public event EventHandler<TrackChangedEventArgs> TrackChanged;

        /// <summary>
        /// Raises the <see cref="E:TrackChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="TrackChangedEventArgs"/> instance containing the event data.</param>
        protected void OnTrackChanged(TrackChangedEventArgs args)
        {
            if (this.TrackChanged != null)
            {
                this.TrackChanged(this, args);
            }
        }

        /// <summary>
        /// Gets fired when Spotify's play state changed.
        /// </summary>
        public event EventHandler<PlayStateChangedEventArgs> PlayStateChanged;

        /// <summary>
        /// Raises the <see cref="E:PlayStateChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="PlayStateChangedEventArgs"/> instance containing the event data.</param>
        protected void OnPlayStateChanged(PlayStateChangedEventArgs args)
        {
            if (this.PlayStateChanged != null)
            {
                this.PlayStateChanged(this, args);
            }
        }

        /// <summary>
        /// Gets fired when the volume Spotify is playing with changed.
        /// </summary>
        public event EventHandler<VolumeChangedEventArgs> VolumeChanged;

        /// <summary>
        /// Raises the <see cref="E:VolumeChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="VolumeChangedEventArgs"/> instance containing the event data.</param>
        protected void OnVolumeChanged(VolumeChangedEventArgs args)
        {
            if (this.VolumeChanged != null)
            {
                this.VolumeChanged(this, args);
            }
        }

        /// <summary>
        /// Gets fired when the data fetched from Spotify got an update.
        /// </summary>
        public event EventHandler<DataUpdatedEventArgs> DataUpdated;

        /// <summary>
        /// Raises the <see cref="E:DataUpdated" /> event.
        /// </summary>
        /// <param name="args">The <see cref="DataUpdatedEventArgs"/> instance containing the event data.</param>
        protected void OnDataUpdated(DataUpdatedEventArgs args)
        {
            if (this.DataUpdated != null)
            {
                this.DataUpdated(this, args);
            }
        }

        /// <summary>
        /// Disposes all used objects
        /// </summary>
        public void Dispose()
        {
            this.timer.Stop();
            this.timer.Dispose();
        }
    }
}
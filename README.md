# nSpotify
A simple .NET Library that allows developers to fetch data from the Spotify Client

Intended to be a replacement for the obsolete spotify-local-api, it became probably the easiest way to access the local Spotify client via the Spotify Web Api.
The parsing of the JSON responses is done with the JSON.NET library by James Newton-King. You can find it on [Codeplex](https://json.codeplex.com/) or [NuGet](http://www.nuget.org/packages/Newtonsoft.Json).

The official (german) page is in the [vb-paradise.de](http://www.vb-paradise.de/index.php/Thread/108982-nSpotify-1-0-Einfach-zu-nutzende-Spotify-Api/) forum.



## Class Diagrams
![Main Class Diagram](http://www.vb-paradise.de/index.php/Attachment/30829-MainClassDiagram-png/)
![Data Model Class Diagram](http://www.vb-paradise.de/index.php/Attachment/30827-DataModelClassDiagram-png/)
![Event Model Class Diagram](http://www.vb-paradise.de/index.php/Attachment/30828-EventModelClassDiagram-png/)



## Examples

### Include Library
```c#
using nSpotify;
```

### Get current song
```c#
Status status = Spotify.DataProviderInstance.UpdateStatus(); //Fetch the latest status information from the Spotify client
MessageBox.Show("Currently playing " + status.Track.Name + " by " + status.Track.Artist);
```

### Show a message when the current track changed
```c#
public class Notifier : Form
{
    public Notifier()
    {
        InitializeComponent();
        
        eventProvider = new EventProvider(); //No need to specify any arguments here
        eventProvider.EventSynchronizingObject = this; //Set the SynchronizingObject property to redirect event calls to the main thread
        eventProvider.TrackChanged += eventProvider_TrackChanged; //Add an event handler to the TrackChanged event
        eventProvider.Start(); //Start the event provider
    }
    
    protected EventProvider eventProvider;
    private void eventProvider_TrackChanged(object sender, TrackChangedEventArgs e)
    {
        //Show a message box with 
        MessageBox.Show("The current track changed. Now " + e.CurrentTrack.Name + " is played instead of " + e.LastTrack.Name + ".");
    }
}
```

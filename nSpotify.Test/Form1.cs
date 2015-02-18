using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using nSpotify;

namespace nSpotify.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();



            this.eProvider = new EventProvider(Spotify.DataProviderInstance, 50, this);
            Action<string, object> ReportUpdate = (prop, value) =>
            {
                this.listBox1.Items.Add(prop + " Changed: " + value.ToString());
                this.listBox1.SelectedIndex = listBox1.Items.Count - 1;
            };

            this.eProvider.TrackChanged += (s, arg) => ReportUpdate("Track", provider.CurrentStatus.Track.Name);
            this.eProvider.VolumeChanged += (s, arg) => ReportUpdate("Volume", provider.CurrentStatus.Volume);
            this.eProvider.PlayStateChanged += (s, arg) => ReportUpdate("Play State", provider.CurrentStatus.Playing ? "Playing" : "Paused");

            this.Disposed += Form1_Disposed;
        }

        void Form1_Disposed(object sender, EventArgs e)
        {
            this.eProvider.Dispose();
        }

        private DataProvider provider = Spotify.DataProviderInstance;
        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = provider.UpdateStatus().ToPropertyString();
            Application.DoEvents(); //Awful to look at but hey, it's just a test app
            this.pictureBox1.Image = provider.CurrentStatus.Track.DownloadAlbumArt(Track.AlbumArtSize.Size160);
        }

        private EventProvider eProvider;
        private void button2_Click(object sender, EventArgs e)
        {
            eProvider.Enabled = !eProvider.Enabled;
        }

        
    }

    public static class Extensions
    {
        public static string ToPropertyString(this object arg)
        {
            return arg.ToPropertyString(0);
        }

        private static string ToPropertyString(this object arg, int level)
        {
            StringBuilder output = new StringBuilder();
            foreach(PropertyInfo property in arg.GetType().GetProperties())
            {
                if (property.PropertyType.Assembly == typeof(nSpotify.Spotify).Assembly)
                {
                    output.AppendLine(property.GetValue(arg).ToPropertyString(level + 1));
                }
                else
                {
                    output.AppendLine(string.Format("{0}{1}: {2}", new string(' ', level * 2), property.Name, property.GetValue(arg)));
                }
            }
            return output.ToString();
        }
    }
}

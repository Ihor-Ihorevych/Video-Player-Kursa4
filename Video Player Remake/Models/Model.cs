using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Video_Player_Remake
{
    class Model : INotifyPropertyChanged
    {
        #region Fields
        private MediaElement mediaElement;
        private string _fileName;
        private double _volume, _maxLenght;
        private bool _isPlaying, _repeat;
        private TimeSpan _playerPositionSpan;
        private Timer eventTimer;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        public Model(MediaElement e)
        {
            try
            {
                mediaElement = e;
                mediaElement.LoadedBehavior = MediaState.Manual;
                mediaElement.UnloadedBehavior = MediaState.Manual;
                mediaElement.MediaOpened += Max_Len;
                mediaElement.MediaEnded += Is_Not_Playing;
                mediaElement.Volume = _volume;

                eventTimer = new Timer()
                {
                    Interval = 10,
                    Enabled = false
                };
                eventTimer.Elapsed += Update_Position;
            }
            catch (Exception er) { throw (er); }
        }
        #region Properties
        public string FileName
        {
            get => _fileName;
            set {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
        public double Volume {
            get => Math.Round(_volume);
            set
            {
                mediaElement.Volume = value / 100;
                _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }
        public double MaxLen
        {
            get => _maxLenght;
            set
            {
                _maxLenght = value;
                OnPropertyChanged(nameof(MaxLen));
            }
        }
        public bool isPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(isPlaying));
            }
        }
        public bool Repeat
        {
            get => _repeat;
            set
            {
                _repeat = value;
                OnPropertyChanged(nameof(Repeat));
            } 
        }
        public TimeSpan PositionTimeSpan {
            get => _playerPositionSpan;
            set
            {
                _playerPositionSpan = value;
                OnPropertyChanged(nameof(PositionTimeSpan));
                OnPropertyChanged(nameof(PositionDouble));
            }
        }
        public double PositionDouble
        {
            get => PositionTimeSpan.TotalSeconds;
            set {
                mediaElement.Position = TimeSpan.FromSeconds(Convert.ToInt32(value));
                PositionTimeSpan = TimeSpan.FromSeconds(Convert.ToInt32(value));
                OnPropertyChanged(nameof(PositionDouble));
            }
        }
        #endregion
        #region Methods
        public void Stop()
        {
            try
            {
                mediaElement.Stop();
                eventTimer.Stop();
                isPlaying = false;
            }
            catch (Exception e) { throw (e); }
        }
        public void Play()
        {
            try
            {
                if (!isPlaying && mediaElement.NaturalDuration.HasTimeSpan)
                {
                    mediaElement.Play();
                    eventTimer.Start();
                    isPlaying = true;
                    return;
                }
                mediaElement.Play();
                mediaElement.Pause();
            }
            catch (Exception e) { throw (e); }
        }
        public void Pause()
        {
            try
            {
                if (isPlaying)
                {
                    mediaElement.Pause();
                    eventTimer.Stop();
                    isPlaying = false;
                }
            }
            catch (Exception e) { throw (e); }
        }
        private void Update_Position(object source, EventArgs e)
        {
            try
            {
                if(Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() => {
                    PositionTimeSpan = mediaElement.Position;
                }));
            }
            catch (Exception) { Application.Current.Shutdown(); }
        }
        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Media |*.mp4;*.mp3;*.mpg;*.mpeg;*.avi;*.wav;*.wma",
                Title = "Choose media file"
            };
            if (openFileDialog.ShowDialog() == true && openFileDialog.FileName != "")
            {
                try
                {
                    isPlaying = false;
                    var uri = new Uri(openFileDialog.FileName);
                    FileName = openFileDialog.SafeFileName;
                    mediaElement.Source = uri;
                    Play();
                }
                catch (Exception e) {
                    throw (e);
                }
            }
        }
        private void Max_Len(object source, EventArgs e) {
            if (mediaElement.NaturalDuration.HasTimeSpan) MaxLen = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
        }
        private void Is_Not_Playing(object source, EventArgs e) {
            Stop();
            if (Repeat) Play();
        }
      
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}

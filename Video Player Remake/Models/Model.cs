using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Video_Player_Remake.Models
{
    class Model : INotifyPropertyChanged
    {
        private MediaElement mediaElement;
        private string _fileName = "No file";
        private double _volume = 5;
        private double _maxLenght;
        private double _playerPositionDouble;
        private bool _isPlaying;
        private TimeSpan _playerPositionSpan;
        private TimeSpan ZeroSpan = new TimeSpan(0);
        private Timer eventTimer;
        public event PropertyChangedEventHandler PropertyChanged;
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
        public TimeSpan PositionTimeSpan {
            get => _playerPositionSpan;
            set
            {
                _playerPositionSpan = value;
                OnPropertyChanged(nameof(PositionTimeSpan));
            }
        }
        public double PositionDouble
        {
            get => _playerPositionDouble;
            set {
                _playerPositionDouble = value;
                mediaElement.Position = new TimeSpan(0, 0, Convert.ToInt32(value));
                OnPropertyChanged(nameof(PositionDouble));
            }
        }
        public void Stop()
        {
            try
            {
                mediaElement.Stop();
                eventTimer.Stop();
                mediaElement.Position = ZeroSpan;
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
                Filter = "Media (mp4, mp3, mpg, mpeg)|*.mp4;*.mp3;*.mpg;*.mpeg;",
                Title = "Choose media file"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var uri = new Uri(openFileDialog.FileName);
                    FileName = openFileDialog.SafeFileName;
                    mediaElement.Source = uri;
                    isPlaying = false;
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
        }
      
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }   
    }
}

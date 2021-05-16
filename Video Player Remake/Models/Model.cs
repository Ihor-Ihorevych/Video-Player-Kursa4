using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Media_Player_Remake
{
    class Model : INotifyPropertyChanged
    {
        #region Fields
        private protected MediaElement mediaElement;
        private protected string _fileName;
        private protected double _volume, _maxLenght, _lastVolume;
        private protected bool _isPlaying, _repeat, _isMuted = false;
        private protected TimeSpan _playerPositionSpan;
        private protected Timer eventTimer;
        private protected Action _setText;
        private protected List<string> _timeProps = new List<string>() { nameof(PositionTimeSpan), nameof(PositionDouble) };

        #endregion
        public Model(MediaElement e)
        {
            try
            {
                _setText = new Action(() => PositionTimeSpan = mediaElement.Position);
                mediaElement = e;
                mediaElement.LoadedBehavior = MediaState.Manual;
                mediaElement.UnloadedBehavior = MediaState.Manual;
                mediaElement.MediaOpened += Max_Len;
                mediaElement.MediaEnded += Is_Not_Playing;
                mediaElement.Volume = _volume;
                eventTimer = new Timer { Interval = 150, Enabled = false };
                eventTimer.Elapsed += Update_Position;
            }
            catch (Exception er) { throw er; }
        }
        #region Properties
        public string FileName { get => _fileName; set { _fileName = value; OnPropertyChanged(nameof(FileName)); } }
        public double Volume { get => Math.Round(_volume); set { mediaElement.Volume = value / 100; _volume = value; OnPropertyChanged(nameof(Volume)); } }
        public double MaxLen { get => _maxLenght; set { _maxLenght = value; OnPropertyChanged(nameof(MaxLen)); } }
        public bool isPlaying { get => _isPlaying; set { _isPlaying = value; OnPropertyChanged(nameof(isPlaying)); } }
        public bool Repeat { get => _repeat; set { _repeat = value; OnPropertyChanged(nameof(Repeat)); } }
        public TimeSpan PositionTimeSpan { get => _playerPositionSpan; set { _playerPositionSpan = value; Update_Props(_timeProps); } }
        public double PositionDouble { get => Math.Round(PositionTimeSpan.TotalSeconds); set { mediaElement.Play(); mediaElement.Position = TimeSpan.FromSeconds(Convert.ToInt32(value)); PositionTimeSpan = TimeSpan.FromSeconds(Convert.ToInt32(value)); mediaElement.Play(); OnPropertyChanged(nameof(PositionDouble)); } }
        #endregion
        #region Methods
        public void Stop()
        {
            try
            {
                FileName = FileName.Split('|')[0];
                mediaElement.Stop();
                eventTimer.Stop();
                isPlaying = false;
            }
            catch (Exception e) { throw e; }
        }
        public void Play()
        {
            try
            {
                if (!isPlaying && mediaElement.NaturalDuration.HasTimeSpan)
                {
                    mediaElement.Play();
                    FileName = $"{FileName.Replace(" ", "").Split('|')[0]} | ▶️";
                    eventTimer.Start();
                    isPlaying = true;
                    return;
                }
                else
                {
                    mediaElement.Pause();
                    if (FileName != "No File") FileName = $"{FileName.Replace(" ", "").Split('|')[0]} | ⏸️";
                    eventTimer.Stop();
                    isPlaying = false;
                }
            }
            catch (Exception e) { throw e; }
        }
        public void Mute()
        {
            if (_isMuted) { Volume = _lastVolume; _isMuted = !_isMuted; }
            else { _lastVolume = Volume; _isMuted = !_isMuted; Volume = 0; }
        }
        public void ChangePosition(TimeSpan t) => mediaElement.Position += t;
        public void OpenFile()
        {
            OpenFileDialog o = new OpenFileDialog { Filter = "Media |*.mp4;*.mp3;*.mpg;*.mpeg;*.avi;*.wav;*.wma; *.mkv", Title = "Choose media file" };
            if (o.ShowDialog() == true && o.FileName != "")
            {
                try
                {
                    isPlaying = false;
                    FileName = o.SafeFileName;
                    mediaElement.Source = new Uri(o.FileName);
                    for (int i = 0; i < 3; i++) Play();
                }
                catch (Exception e) { throw e; }
            }
        }
        private protected void Update_Position(object source, EventArgs e)
        {
            try
            {
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, _setText); ;
            }
            catch (Exception) { Application.Current.Shutdown(); }
        }
        private protected void Update_Props(List<string> props) => props.ForEach(x => OnPropertyChanged(x));
        private protected void Max_Len(object source, EventArgs e) { if (mediaElement.NaturalDuration.HasTimeSpan) MaxLen = mediaElement.NaturalDuration.TimeSpan.TotalSeconds; }
        private protected void Is_Not_Playing(object source, EventArgs e) { Stop(); if (Repeat) Play(); }
        public event PropertyChangedEventHandler PropertyChanged;
        private protected void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
    }
}

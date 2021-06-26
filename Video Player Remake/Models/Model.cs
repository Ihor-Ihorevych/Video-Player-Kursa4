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
        private protected MediaElement _mediaelement;
        private protected double _volume, _lastVolume;
        private protected bool _isMuted;
        private protected TimeSpan _playerPositionSpan;
        private protected Timer _eventTimer = new() { Interval = 10, Enabled = false };
        private protected Action _setTimerPosition;
        private protected List<string> _timeProps = new() { nameof(PositionTimeSpan), nameof(PositionDouble) };

        #endregion
        public Model(MediaElement e)
        {
            try
            {
                _setTimerPosition = new Action(() => PositionTimeSpan = _mediaelement.Position);
                _mediaelement = e;
                _mediaelement.LoadedBehavior = MediaState.Manual;
                _mediaelement.UnloadedBehavior = MediaState.Manual;
                _mediaelement.MediaOpened += MediaOpened_Handler;
                _mediaelement.MediaEnded += MediaEnded_Handler;
                _mediaelement.Volume = _volume;
                _eventTimer.Elapsed += Update_Position;
            }
            catch (Exception) { throw; }
        }
        #region Properties
        public string FileName { get; set; }
        public double Volume
        {
            get => Math.Round(_volume);
            set
            {
                _mediaelement.Volume = value / 100;
                _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }
        public double MaxLenght { get; set; }
        public bool isPlaying { get; set; }
        public bool Repeat { get; set; }
        public TimeSpan PositionTimeSpan
        {
            get => _playerPositionSpan;
            set
            {
                _playerPositionSpan = value;
                Update_Props(_timeProps);
            }
        }
        public double PositionDouble
        {
            get => Math.Round(PositionTimeSpan.TotalSeconds);
            set
            {
                _mediaelement.Position = TimeSpan.FromSeconds(Convert.ToInt32(value));
                PositionTimeSpan = TimeSpan.FromSeconds(Convert.ToInt32(value));
                _mediaelement.Play();
                OnPropertyChanged(nameof(PositionDouble));
            }
        }
        #endregion
        #region Methods
        public void Stop()
        {
            try
            {
                FileName = FileName.Split('|')[0];
                _mediaelement.Stop();
                _eventTimer.Stop();
                isPlaying = false;
            }
            catch (Exception) { throw; }
        }
        public void Play()
        {
            try
            {
                if (!isPlaying && _mediaelement.NaturalDuration.HasTimeSpan)
                {
                    _mediaelement.Play();
                    FileName = $"{FileName.Replace(" ", "").Split('|')[0]} | ▶️";
                    _eventTimer.Start();
                    isPlaying = true;
                }
                else
                {
                    _mediaelement.Pause();
                    if (FileName != "No File")
                        FileName = $"{FileName.Replace(" ", "").Split('|')[0]} | ⏸️";
                    _eventTimer.Stop();
                    isPlaying = false;
                }
            }
            catch (Exception) { throw; }
        }
        public void Mute()
        {
            if (_isMuted)
            {
                Volume = _lastVolume;
                _isMuted = !_isMuted;
            }
            else
            {
                _lastVolume = Volume;
                _isMuted = !_isMuted;
                Volume = 0;
            }
        }
        public void ChangePosition(TimeSpan t) => _mediaelement.Position += t;
        public void OpenFile()
        {
            OpenFileDialog o = new OpenFileDialog { Filter = "Media |*.mp4;*.mp3;*.mpg;*.mpeg;*.avi;*.wav;*.wma; *.mkv", Title = "Choose media file" };
            if (o.ShowDialog() == true && o.FileName != "")
            {
                try
                {
                    isPlaying = false;
                    FileName = o.SafeFileName;
                    _mediaelement.Source = new Uri(o.FileName);
                    Play();
                }
                catch (Exception) { throw; }
            }
        }
        private protected void Update_Position(object source, EventArgs e)
        {
            try
            {
                if (Application.Current is not null)
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, _setTimerPosition);
            }
            catch (Exception) { Application.Current.Shutdown(); }
        }
        private protected void Update_Props(List<string> props) => props.ForEach(x => OnPropertyChanged(x));
        private protected void MediaOpened_Handler(object source, EventArgs e)
        {
            if (_mediaelement.NaturalDuration.HasTimeSpan)
                MaxLenght = _mediaelement.NaturalDuration.TimeSpan.TotalSeconds;
        }
        private protected void MediaEnded_Handler(object source, EventArgs e)
        {
            Stop();
            if (Repeat) 
                Play();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private protected void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
    }
}

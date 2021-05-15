using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using Video_Player_Remake.Models;

namespace Media_Player_Remake
{
    class ViewModel : INotifyPropertyChanged
    {
        #region Fields
        private bool _darkMode;
        private bool _fullScreen;
        private BrushConverter converter = new BrushConverter();
        private Brush darkColor = Brushes.Black,
                    brightColor = Brushes.White;
        #endregion
        #region Properties
        public WindowState FullScreen
        {
            get => _fullScreen ? WindowState.Maximized : WindowState.Normal;
        }
        public WindowStyle Borders
        {
            get => _fullScreen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
        }
        public bool DarkMode
        {
            get => _darkMode;
            set
            {
                _darkMode = value;
                OnPropertyChanged(nameof(DarkMode));
                Save_Settings();
                Update_Props();
            }
        }
        public Brush ButtonColor
        {
            get => DarkMode ? Brushes.White : Brushes.Transparent;
        }
        public Brush PrimaryColor
        {
            get => DarkMode ? darkColor : brightColor;
        }
        public Brush PrimaryTextColor
        {
            get => DarkMode ? brightColor : darkColor;
        }
        public string Opacity
        {
            get => DarkMode ? "0.3" : "1";
        }
        #endregion
        public ViewModel(MediaElement mediaElement)
        {
            _player = new Model(mediaElement)
            {
                Volume = 15,
                FileName = "No file"
            };
            Load_Settings();
        }
        private Model _player { get; set; }
        public Model Player
        {
            get => _player;
        }
        #region Commands
        private Commands.RelayCommand stop;
        public Commands.RelayCommand Stop
        {
            get
            {
                return stop ?? (stop = new Commands.RelayCommand(player =>
                {
                    (player as Model).Stop();
                }
                ));
            }
        }
        private Commands.RelayCommand play;
        public Commands.RelayCommand Play
        {
            get
            {
                return play ?? (play = new Commands.RelayCommand(player =>
                {
                    (player as Model).Play();
                }
                ));
            }
        }
        private Commands.RelayCommand pause;
        public Commands.RelayCommand Pause
        {
            get
            {
                return pause ?? (pause = new Commands.RelayCommand(player =>
                {
                    (player as Model).Pause();
                }
                ));
            }
        }

        private Commands.RelayCommand openFile;
        public Commands.RelayCommand OpenFile
        {
            get
            {
                return openFile ?? (openFile = new Commands.RelayCommand(player =>
                {
                    (player as Model).OpenFile();
                }));
            }
        }
        private Commands.RelayCommand colorPicker; // ain't implified
        public Commands.RelayCommand ColorPicker
        {
            get
            {
                return colorPicker ?? (colorPicker = new Commands.RelayCommand(obj =>
                {
                    var settings = new ColorPicker();
                    if (settings.ShowDialog() == true)
                    {

                    }
                    Save_Settings();
                    Update_Props();
                }));

            }
        }
        private Commands.RelayCommand fullscreen;
        public Commands.RelayCommand FullScreenSet
        {
            get
            {
                return fullscreen ?? (fullscreen = new Commands.RelayCommand(obj =>
                {
                    _fullScreen = !_fullScreen;
                    Update_Props();
                }));
            }
        }

        #endregion
        #region Methods
        private void Save_Settings()
        {
            try
            {
                XDocument xdoc = new XDocument();
                XElement Settings = new XElement("Settings");
                Settings.Add(new XAttribute("isDark", DarkMode), new XAttribute("Primary", brightColor));
                xdoc.Add(Settings);
                xdoc.Save("player.settings");
            }
            catch (Exception e) { throw e; }
        }
        private void Load_Settings()
        {
            try
            {
                var doc = XDocument.Load("player.settings");
                DarkMode = Convert.ToBoolean(doc.Element("Settings").Attribute("isDark").Value);
                brightColor = (Brush)converter.ConvertFromString(doc.Element("Settings").Attribute("Primary").Value);
            }
            catch (Exception) { Save_Settings(); }
        }
        private void Update_Props()
        {
            foreach (var prop in new string[] {
                "PrimaryColor",
                "PrimaryTextColor",
                "Opacity",
                "FullScreen",
                "Borders",
                "ButtonColor"
            }) OnPropertyChanged(prop);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private protected void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
    }
}

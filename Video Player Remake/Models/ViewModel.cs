using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace Media_Player_Remake
{
    class ViewModel : INotifyPropertyChanged
    {
        #region Fields
        private bool _darkMode;
        private Brush darkColor = Brushes.Black,
                    brightColor = Brushes.White;
        #endregion
        #region Properties
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
                return openFile ?? (openFile = new Commands.RelayCommand(player => {
                    (player as Model).OpenFile();
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
                Settings.Add(new XAttribute("isDark", DarkMode));
                xdoc.Add(Settings);
                xdoc.Save("player.settings");
            }
            catch (Exception e) { throw e; }
        }
        private void Load_Settings()
        {
            try
            {
                DarkMode = Convert.ToBoolean(XDocument.Load("player.settings").Element("Settings").Attribute("isDark").Value);
            }
            catch (Exception) { Save_Settings(); }
        }
        private void Update_Props()
        {
            foreach (var prop in new string[] { "PrimaryColor", "PrimaryTextColor", "Opacity" }) OnPropertyChanged(prop);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}

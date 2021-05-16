using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace Media_Player_Remake
{
    class ViewModel : INotifyPropertyChanged
    {
        public ViewModel(MediaElement e) { Player = new Model(e) { Volume = 75, FileName = "No File" }; Load_Settings(); }
        #region Fields
        private protected List<string> _emojis = new List<string>() { "F/F11 - 📺", "", "Ctrl+D - 🌙", "Ctrl+O - 📂", "Ctrl+R - 🔁", "Ctrl+S - ⏹️", "Ctrl+M/🖱 - 🔇", "Ctrl+H - ⛔", "", "Spacebar/🖱- 🎬", "Home/End - 🎬", "↕️↔️ - 🎬" },
            _getprops = new List<string>() {
                nameof(PrimaryColor),
                nameof(PrimaryTextColor),
                nameof(DockPanelOpacity),
                nameof(DarkMode),
                nameof(FullScreen),
                nameof(WindowBorders),
                nameof(ButtonColor),
                nameof(Cursor),
                nameof(DockPanelVisibility),
                nameof(PanelHidden),
                nameof(PanelHiddenShow),
                nameof(RestoreButtonOpacity) };
        private protected bool _darkMode, _fullScreen, _cursorHidden, _panelHidden;
        private protected BrushConverter _converter = new BrushConverter();
        private protected Brush _darkColor = Brushes.Black,
                    _brightColor = Brushes.White;
        private protected Model _player;
        #endregion
        #region Properties
        public bool DarkMode { get => _darkMode; set { _darkMode = value; Update_Props(_getprops); Save_Settings(); } }
        public string PanelHiddenShow => _panelHidden ? "Auto" : "0";
        public string PanelHidden => _panelHidden ? "0" : "Auto";
        public string RestoreButtonOpacity => _fullScreen ? "0.16" : "0.25";
        public string DockPanelOpacity => DarkMode ? "0.33" : "1";
        public Visibility RestoreButton => _panelHidden ? Visibility.Visible : Visibility.Hidden;
        public Visibility DockPanelVisibility => _panelHidden ? Visibility.Hidden : Visibility.Visible;
        public Cursor Cursor => _cursorHidden ? Cursors.None : Cursors.Arrow;
        public WindowState FullScreen => _fullScreen ? WindowState.Maximized : WindowState.Normal;
        public WindowStyle WindowBorders => _fullScreen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
        public Brush ButtonColor => DarkMode ? _brightColor : Brushes.Transparent;
        public Brush PrimaryColor => DarkMode ? _darkColor : _brightColor;
        public Brush PrimaryTextColor => DarkMode ? _brightColor : _darkColor;
        #endregion

        public Model Player { get => _player; set { _player = value; OnPropertyChanged(nameof(Player)); } }

        #region Commands
        private protected Commands.RelayCommand stop, help, endBtn, homeBtn, hidemouse, setdark, repeat, fullscreen, openFile, play, mute, hidepanel, jumppos;
        public Commands.RelayCommand Help => help ?? (help = new Commands.RelayCommand(obj => { var txt = String.Empty; _emojis.ForEach(x => txt += $"{x}\n"); MessageBox.Show(txt, "?"); }));
        public Commands.RelayCommand Stop => stop ?? (stop = new Commands.RelayCommand(player => (player as Model).Stop()));
        public Commands.RelayCommand Play => play ?? (play = new Commands.RelayCommand(player => (player as Model).Play()));
        public Commands.RelayCommand OpenFile => openFile ?? (openFile = new Commands.RelayCommand(player => (player as Model).OpenFile()));
        public Commands.RelayCommand FullScreenSet => fullscreen ?? (fullscreen = new Commands.RelayCommand(obj => { _fullScreen = !_fullScreen; Update_Props(_getprops); }));
        public Commands.RelayCommand Repeat => repeat ?? (repeat = new Commands.RelayCommand(player => (player as Model).Repeat = !(player as Model).Repeat));
        public Commands.RelayCommand SetDark => setdark ?? (setdark = new Commands.RelayCommand(obj => DarkMode = !DarkMode));
        public Commands.RelayCommand HideMouse => hidemouse ?? (hidemouse = new Commands.RelayCommand(obj => { _cursorHidden = !_cursorHidden; Update_Props(_getprops); }));
        public Commands.RelayCommand HomeButton => homeBtn ?? (homeBtn = new Commands.RelayCommand(player => (player as Model).PositionDouble = 0));
        public Commands.RelayCommand EndButton => endBtn ?? (endBtn = new Commands.RelayCommand(player => (player as Model).PositionDouble = (player as Model).MaxLen));
        public Commands.RelayCommand Mute => mute ?? (mute = new Commands.RelayCommand(player => (player as Model).Mute()));
        public Commands.RelayCommand HidePanel => hidepanel ?? (hidepanel = new Commands.RelayCommand(obj => { _panelHidden = !_panelHidden; Save_Settings(); Update_Props(_getprops); }));
        public Commands.RelayCommand ChangePos => jumppos ?? (jumppos = new Commands.RelayCommand(obj => { Player.ChangePosition(TimeSpan.FromSeconds(Int32.Parse(obj.ToString()))); }));
        #endregion
        #region Methods
        private protected void Save_Settings()
        {
            try
            {
                XDocument xdoc = new XDocument();
                XElement Settings = new XElement("Settings");
                Settings.Add(new XAttribute("isDark", DarkMode), new XAttribute("Primary", _brightColor), new XAttribute("PanelHidden", _panelHidden));
                xdoc.Add(Settings);
                xdoc.Save("player.settings");
            }
            catch (Exception e) { throw e; }
        }
        private protected void Load_Settings()
        {
            try
            {
                var doc = XDocument.Load("player.settings");
                _darkMode = Convert.ToBoolean(doc.Element("Settings").Attribute("isDark").Value);
                _brightColor = (Brush)_converter.ConvertFromString(doc.Element("Settings").Attribute("Primary").Value);
                _panelHidden = Convert.ToBoolean(doc.Element("Settings").Attribute("PanelHidden").Value);
                Update_Props(_getprops);
            }
            catch (Exception) { Save_Settings(); }
        }
        private protected void Update_Props(List<string> props) => props.ForEach(prop => OnPropertyChanged(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        private protected void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
    }
}

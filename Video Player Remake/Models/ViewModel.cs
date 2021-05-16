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
        private protected List<string> _emojis = new List<string>() { 
            "F/F11 - 📺\n", 
            "Ctrl+D - 🌙", 
            "Ctrl+O - 📂", 
            "Ctrl+R - 🔁", 
            "Ctrl+S - ⏹️", 
            "Ctrl+M/🖱 - 🔇", 
            "Ctrl+H - ⛔\n", 
            "Spacebar/🖱- 🎬", 
            "Home/End - 🎬", 
            "↕️↔️ - 🎬", 
            "🖱📜 - 🖱⛔" },
            _getprops = new List<string>() {
                nameof(PrimaryDockPanelColor),
                nameof(PrimaryForegroundColor),
                nameof(DockPanelOpacity),
                nameof(DarkMode),
                nameof(FullScreen),
                nameof(WindowBorders),
                nameof(ButtonsColor),
                nameof(Cursor),
                nameof(DockPanelVisibility),
                nameof(DefaultRowHeight),
                nameof(AdditionalRowHeight),
                nameof(RestoreButtonOpacity)
            };
        private protected bool _darkMode, _isFullScreen, _cursorHidden, _panelHidden;
        private protected BrushConverter _converter = new BrushConverter();
        private protected Brush _darkColor = Brushes.Black, _brightColor = Brushes.White;
        private protected Model _player;
        #endregion
        #region Properties
        public Model Player { get => _player; set { _player = value; OnPropertyChanged(nameof(Player)); } }
        public bool DarkMode { get => _darkMode; set { _darkMode = value; Update_Props(_getprops); Save_Settings(); } }
        public string AdditionalRowHeight => _panelHidden ? "Auto" : "0";
        public string DefaultRowHeight => _panelHidden ? "0" : "Auto";
        public string RestoreButtonOpacity => _isFullScreen ? "0.16" : "0.25";
        public string DockPanelOpacity => DarkMode ? "0.33" : "1";
        public Visibility DockPanelVisibility => _panelHidden ? Visibility.Hidden : Visibility.Visible;
        public Cursor Cursor => _cursorHidden ? Cursors.None : Cursors.Arrow;
        public WindowState FullScreen => _isFullScreen ? WindowState.Maximized : WindowState.Normal;
        public WindowStyle WindowBorders => _isFullScreen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
        public Brush ButtonsColor => DarkMode ? _brightColor : Brushes.Transparent;
        public Brush PrimaryDockPanelColor => DarkMode ? _darkColor : _brightColor;
        public Brush PrimaryForegroundColor => DarkMode ? _brightColor : _darkColor;
        #endregion
        

        #region Commands
        private protected Commands.RelayCommand _stop, _help, _jumpToEnd, _jumpToStart, _hidemouse, _swapdark, _repeat, _toggleFullScreen, _openfile, _play, _mute, _hidecontrolpanel, _changePos;
        public Commands.RelayCommand Help => _help ?? (_help = new Commands.RelayCommand(obj => { var txt = String.Empty; _emojis.ForEach(x => txt += $"{x}\n"); MessageBox.Show(txt, "?"); }));
        public Commands.RelayCommand Stop => _stop ?? (_stop = new Commands.RelayCommand(player => (player as Model).Stop()));
        public Commands.RelayCommand Play => _play ?? (_play = new Commands.RelayCommand(player => (player as Model).Play()));
        public Commands.RelayCommand OpenFile => _openfile ?? (_openfile = new Commands.RelayCommand(player => (player as Model).OpenFile()));
        public Commands.RelayCommand ToggleFullScreen => _toggleFullScreen ?? (_toggleFullScreen = new Commands.RelayCommand(obj => { _isFullScreen = !_isFullScreen; Update_Props(_getprops); }));
        public Commands.RelayCommand Repeat => _repeat ?? (_repeat = new Commands.RelayCommand(player => (player as Model).Repeat = !(player as Model).Repeat));
        public Commands.RelayCommand SwapDark => _swapdark ?? (_swapdark = new Commands.RelayCommand(obj => DarkMode = !DarkMode));
        public Commands.RelayCommand HideMouse => _hidemouse ?? (_hidemouse = new Commands.RelayCommand(obj => { _cursorHidden = !_cursorHidden; Update_Props(_getprops); }));
        public Commands.RelayCommand JumpToStart => _jumpToStart ?? (_jumpToStart = new Commands.RelayCommand(player => (player as Model).PositionDouble = 0));
        public Commands.RelayCommand JumpToEnd => _jumpToEnd ?? (_jumpToEnd = new Commands.RelayCommand(player => (player as Model).PositionDouble = (player as Model).MaxLen));
        public Commands.RelayCommand Mute => _mute ?? (_mute = new Commands.RelayCommand(player => (player as Model).Mute()));
        public Commands.RelayCommand HideControlPanel => _hidecontrolpanel ?? (_hidecontrolpanel = new Commands.RelayCommand(obj => { _panelHidden = !_panelHidden; Save_Settings(); Update_Props(_getprops); }));
        public Commands.RelayCommand ChangePos => _changePos ?? (_changePos = new Commands.RelayCommand(obj => { Player.ChangePosition(TimeSpan.FromSeconds(Int32.Parse(obj.ToString()))); }));
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

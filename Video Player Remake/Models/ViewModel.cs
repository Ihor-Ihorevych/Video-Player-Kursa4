using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Media_Player_Remake.Commands;

namespace Media_Player_Remake
{
    class ViewModel : INotifyPropertyChanged
    {
        public ViewModel(MediaElement mediaElement)
        {
            Player = new Model(mediaElement) { Volume = 65, FileName = "No File" };
        }
        #region Fields
        private protected List<string> _emojis = new()
        {
            "F/F11 - 📺\n", // Separator for new group 
            "Ctrl+D - 🌙",
            "Ctrl+O - 📂",
            "Ctrl+R - 🔁",
            "Ctrl+S - ⏹️",
            "Ctrl+M/🖱 - 🔇",
            "Ctrl+H - ⛔\n",
            "Spacebar/🖱- 🎬",
            "Home/End - 🎬",
            "↕️↔️ - 🎬",
            "🖱📜 - 🖱⛔"
        },

            _allProps = new()
            {
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
        private protected bool _isFullScreened,
            _cursorHidden,
            _panelHidden;
        private protected BrushConverter _converter = new BrushConverter();
        private protected Brush _darkColor = Brushes.Black,
            _brightColor = Brushes.White;
        #endregion
        #region Properties
        public Model Player { get; set; }
        public bool DarkMode { get; set; }
        public string AdditionalRowHeight => _panelHidden ? "Auto" : "0";
        public string DefaultRowHeight => _panelHidden ? "0" : "Auto";
        public string RestoreButtonOpacity => _isFullScreened ? "0.16" : "0.25";
        public string DockPanelOpacity => DarkMode ? "0.33" : "1";
        public Visibility DockPanelVisibility => _panelHidden ? Visibility.Hidden : Visibility.Visible;
        public Cursor Cursor => _cursorHidden ? Cursors.None : Cursors.Arrow;
        public WindowState FullScreen => _isFullScreened ? WindowState.Maximized : WindowState.Normal;
        public WindowStyle WindowBorders => _isFullScreened ? WindowStyle.None : WindowStyle.SingleBorderWindow;
        public Brush ButtonsColor => DarkMode ? _brightColor : Brushes.Transparent;
        public Brush PrimaryDockPanelColor => DarkMode ? _darkColor : _brightColor;
        public Brush PrimaryForegroundColor => DarkMode ? _brightColor : _darkColor;
        #endregion
        #region Commands

        private protected RelayCommand
            _help,
            _hidemouse,
            _swapdark,
            _toggleFullScreen,
            _hidecontrolpanel,
            _changePos;
        public RelayCommand JumpToStart { get; init; } = new(obj => (obj as Model).PositionDouble = 0);
        public RelayCommand JumpToEnd { get; init; } = new(obj => (obj as Model).PositionDouble = (obj as Model).MaxLenght);
        public RelayCommand Mute { get; init; } = new(obj => (obj as Model).Mute());
        public RelayCommand Stop { get; init; } = new(obj => (obj as Model).Stop());
        public RelayCommand Play { get; init; } = new(obj => (obj as Model).Play());
        public RelayCommand OpenFile { get; init; } = new(obj => (obj as Model).OpenFile());
        public RelayCommand Repeat { get; init; } = new(obj => (obj as Model).Repeat = !(obj as Model).Repeat);
        public RelayCommand SwapDark => _swapdark ?? (_swapdark = new(obj => DarkMode = !DarkMode));
        public RelayCommand HideMouse => _hidemouse ?? (_hidemouse = new(obj => { _cursorHidden = !_cursorHidden; Update_Props(_allProps); }));
        public RelayCommand Help => _help ?? (_help = new(obj => { MessageBox.Show(String.Join("\n", _emojis), "?"); }));
        public RelayCommand HideControlPanel => _hidecontrolpanel ?? (_hidecontrolpanel = new(obj => { _panelHidden = !_panelHidden; Update_Props(_allProps); }));
        public RelayCommand ChangePos => _changePos ?? (_changePos = new(obj => { Player.ChangePosition(TimeSpan.FromSeconds(Int32.Parse(obj.ToString()))); }));
        public RelayCommand ToggleFullScreen => _toggleFullScreen ?? (_toggleFullScreen = new(obj => { _isFullScreened = !_isFullScreened; Update_Props(_allProps); }));
        #endregion
        #region Methods
        private protected void Update_Props(List<string> props) => props.ForEach(prop => OnPropertyChanged(prop));

        public event PropertyChangedEventHandler PropertyChanged;
        private protected void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
    }
}

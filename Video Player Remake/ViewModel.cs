using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Video_Player_Remake
{
    class ViewModel : INotifyPropertyChanged
    {
        public ViewModel(MediaElement mediaElement)
        {
            _player = new Models.PlayerModel(mediaElement);
        }

        private Models.PlayerModel _player { get; set; }
        public Models.PlayerModel Player
        {
            get => _player;
        }
        
        private Commands.RelayCommand stop;
        public Commands.RelayCommand Stop
        {
            get
            {
                return stop ?? (stop = new Commands.RelayCommand(player => {
                    (player as Models.PlayerModel).Stop(); 
                }, (player) => Player.isPlaying));
            }
        }
        private Commands.RelayCommand play;
        public Commands.RelayCommand Play
        {
            get
            {
                return play ?? (play = new Commands.RelayCommand(player => {
                    var pl = player as Models.PlayerModel;
                    (player as Models.PlayerModel).Play();
                }, (player) => !Player.isPlaying ));
            }
        }
        private Commands.RelayCommand pause;
        public Commands.RelayCommand Pause
        {
            get
            {
                return pause ?? (pause = new Commands.RelayCommand(player => {
                    (player as Models.PlayerModel).Pause();
                }, (player) => Player.isPlaying));
            }
        }

        private Commands.RelayCommand openFile;
        public Commands.RelayCommand OpenFile
        {
            get
            {
                return openFile ?? (openFile = new Commands.RelayCommand(player => {
                    (player as Models.PlayerModel).OpenFile();
                }));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            } 
        }
    }
}

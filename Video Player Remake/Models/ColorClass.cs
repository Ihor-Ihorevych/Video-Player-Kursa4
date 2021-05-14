using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Video_Player_Remake.Models
{
    public class ColorClass
    {
        private string _name;
        private Brush _brush;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        public Brush Brush
        {
            get => _brush;
            set { _brush = value; OnPropertyChanged(nameof(Brush)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

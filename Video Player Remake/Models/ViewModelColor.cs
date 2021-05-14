using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Media_Player_Remake.Commands;
using System.Windows.Media;

namespace Video_Player_Remake.Models
{
    public class ViewModelColor
    {
        public ViewModelColor()
        {
            Colors = new ObservableCollection<ColorClass>();
            var converter = new BrushConverter();
            var colorNames = typeof(Brushes)
            .GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var color in colorNames)
            {
                dynamic name = color.Name;
                name = converter.ConvertFromString(color.Name);
                Colors.Add(new ColorClass { Name = color.Name, Brush = name });
            }
        }
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                OnPropertyChanged(nameof(DialogResult));
            }
        }
        private ObservableCollection<ColorClass> _colors;
        private ColorClass _chosen;
        public ObservableCollection<ColorClass> Colors
        {
            get => _colors;
            set
            {
                _colors = value;
                OnPropertyChanged(nameof(Colors));
            }
        }
        public ColorClass Chosen
        {
            get => _chosen;
            set
            {
                _chosen = value;
                OnPropertyChanged(nameof(Chosen));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Video_Player_Remake.Models
{
    /// <summary>
    /// Логика взаимодействия для ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        
        public ColorPicker()
        {
            InitializeComponent();
            DataContext = new ViewModelColor();
        }
    }
}

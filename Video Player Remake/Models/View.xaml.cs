using System.Windows;

namespace Media_Player_Remake
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class View : Window
    {
        public View()
        {
            InitializeComponent();
            DataContext = new ViewModel(medieElement);
        }
    }
}

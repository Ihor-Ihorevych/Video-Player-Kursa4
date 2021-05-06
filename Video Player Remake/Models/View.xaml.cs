using System.Windows;

namespace Media_Player_Remake
{
    public partial class View : Window
    {
        public View()
        {
            InitializeComponent();
            DataContext = new ViewModel(medieElement);
        }
    }
}

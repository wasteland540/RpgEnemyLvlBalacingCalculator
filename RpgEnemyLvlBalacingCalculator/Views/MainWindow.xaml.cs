using Microsoft.Practices.Unity;
using RpgEnemyLvlBalacingCalculator.ViewModels;

namespace RpgEnemyLvlBalacingCalculator.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        [Dependency]
        public MainWindowViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}
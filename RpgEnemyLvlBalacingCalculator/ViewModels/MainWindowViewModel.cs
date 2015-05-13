using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Commands.Commands;
using ElzUtilLibary.Wpf.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using RpgEnemyLvlBalacingCalculator.Services;

namespace RpgEnemyLvlBalacingCalculator.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        #region Members

        private readonly ObservableCollection<BaseViewModel> _tabViewModels;
        private ICommand _exitCommand;

        #endregion Members

        public MainWindowViewModel(IMessenger messenger, ICalculationService calculationService)
        {
            _tabViewModels = new ObservableCollection<BaseViewModel>
            {
                new EnemyTabViewModel(messenger, calculationService),
                new CharaktersTabViewModel(messenger),
                new SkillsTabViewModel(messenger, calculationService)
            };
        }

        #region Properties

        public ObservableCollection<BaseViewModel> TabViewModels
        {
            get { return _tabViewModels; }
        }

        public ICommand ExitCommand
        {
            get
            {
                _exitCommand = _exitCommand ?? new DelegateCommand(Exit);
                return _exitCommand;
            }
        }

        #endregion Properties

        #region Private Methods

        private void Exit(object obj)
        {
            Application.Current.Shutdown();
        }

        #endregion Private Methods
    }
}
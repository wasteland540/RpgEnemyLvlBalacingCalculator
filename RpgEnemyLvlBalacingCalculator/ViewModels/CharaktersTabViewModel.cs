using System.Collections.Generic;
using System.Windows.Input;
using Commands.Commands;
using ElzUtilLibary.Wpf.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using RpgEnemyLvlBalacingCalculator.Messages;
using RpgEnemyLvlBalacingCalculator.Model.Units;

namespace RpgEnemyLvlBalacingCalculator.ViewModels
{
    public class CharaktersTabViewModel : BaseViewModel
    {
        #region Members

        private readonly IMessenger _messenger;
        private string _charakterName;
        private List<CharakterClass> _charakters = new List<CharakterClass> {new CharakterClass {Level = 1}};

        private ICommand _saveCommand;
        private CharakterClass _selectedCharakter;

        #endregion Members

        public CharaktersTabViewModel(IMessenger messenger)
        {
            _messenger = messenger;
            _selectedCharakter = _charakters[0];
        }

        #region Properties

        public string CharakterName
        {
            get { return _charakterName; }
            set
            {
                if (value != null && value != _charakterName)
                {
                    _charakterName = value;
                    RaisePropertyChanged("CharakterName");
                }
            }
        }

        public List<CharakterClass> Charakters
        {
            get { return _charakters; }
            set
            {
                if (value != _charakters)
                {
                    _charakters = value;
                    RaisePropertyChanged("Charakters");
                }
            }
        }

        public CharakterClass SelectedCharakter
        {
            get { return _selectedCharakter; }
            set
            {
                if (value != null && value != _selectedCharakter)
                {
                    _selectedCharakter = value;
                    RaisePropertyChanged("SelectedCharakter");
                }
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                _saveCommand = _saveCommand ?? new DelegateCommand(Save);
                return _saveCommand;
            }
        }

        #endregion Properties

        #region Private Methods

        private void Save(object obj)
        {
            foreach (CharakterClass charakter in _charakters)
            {
                charakter.Name = _charakterName;
            }

            _messenger.Send(new CharakterSaveMessage(_charakters));
        }

        #endregion Private Methods
    }
}
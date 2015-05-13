using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Commands.Commands;
using ElzUtilLibary.Math.Formula;
using ElzUtilLibary.Wpf.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using RpgEnemyLvlBalacingCalculator.Messages;
using RpgEnemyLvlBalacingCalculator.Model.Units;
using RpgEnemyLvlBalacingCalculator.Services;
using RpgEnemyLvlBalacingCalculator.Util;

namespace RpgEnemyLvlBalacingCalculator.ViewModels
{
    public class SkillsTabViewModel : BaseViewModel
    {
        #region Members

        private readonly ICalculationService _calculationService;
        private readonly FormulaCalculator _formulaCalculator;
        private ICommand _calculateCommand;
        private List<CharakterClass> _charakters;
        private List<Enemy> _enemies;

        private bool _isLocked;
        private bool _isValid;
        private int _lvlTolerance;

        private ICommand _parseCommand;
        private Dictionary<int, List<CharakterEnemyDmg>> _results = new Dictionary<int, List<CharakterEnemyDmg>>();
        private int _selectedCharakterLevel;
        private int _selectedCharakterIndex;
        private List<CharakterEnemyDmg> _selectedDmgList;
        private string _skillFormula = "a.atk * 4 - b.def * 2";
        private float _variance;

        #endregion Members

        public SkillsTabViewModel(IMessenger messenger, ICalculationService calculationService)
        {
            _calculationService = calculationService;
            _formulaCalculator = new FormulaCalculator();
            messenger.Register<EnemyCalculatedMessage>(this, OnEnemyCalculatedMessage);
            messenger.Register<CharakterSaveMessage>(this, OnCharakterSaveMessage);
        }

        #region Properties

        public string SkillFormula
        {
            get { return _skillFormula; }
            set
            {
                if (value != null && value != _skillFormula)
                {
                    _skillFormula = value;
                    RaisePropertyChanged("SkillFormula");
                }
            }
        }

        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (value != _isLocked)
                {
                    _isLocked = value;
                    RaisePropertyChanged("IsLocked");
                }
            }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (value != _isValid)
                {
                    _isValid = value;
                    RaisePropertyChanged("IsValid");
                }
            }
        }

        public int LvlTolerance
        {
            get { return _lvlTolerance; }
            set
            {
                if (value != _lvlTolerance)
                {
                    _lvlTolerance = value;
                    RaisePropertyChanged("LvlTolerance");
                }
            }
        }

        public ICommand ParseCommand
        {
            get
            {
                _parseCommand = _parseCommand ?? new DelegateCommand(Parse);
                return _parseCommand;
            }
        }

        public ICommand CalculateCommand
        {
            get
            {
                _calculateCommand = _calculateCommand ?? new DelegateCommand(Calculate);
                return _calculateCommand;
            }
        }

        public Dictionary<int, List<CharakterEnemyDmg>> Results
        {
            get { return _results; }
            set
            {
                if (value != null && value != _results)
                {
                    _results = value;
                    RaisePropertyChanged("Results");
                }
            }
        }

        public float Variance
        {
            get { return _variance; }
            set
            {
                _variance = value;
                RaisePropertyChanged("Variance");
            }
        }

        public int SelectedCharakterLevel
        {
            get { return _selectedCharakterLevel; }
            set
            {
                _selectedCharakterLevel = value;

                if (_selectedCharakterIndex != -1)
                {
                    SelectedDmgList = _results[_selectedCharakterLevel];
                }
                else
                {
                    SelectedDmgList = null;
                }

                RaisePropertyChanged("SelectedCharakterLevel");
            }
        }

        public int SelectedCharakterIndex
        {
            get { return _selectedCharakterIndex; }
            set
            {
                if (value != _selectedCharakterIndex)
                {
                    _selectedCharakterIndex = value;
                    RaisePropertyChanged("SelectedCharakterIndex");
                }
            }
        }

        public List<CharakterEnemyDmg> SelectedDmgList
        {
            get { return _selectedDmgList; }
            set
            {
                if (value != _selectedDmgList)
                {
                    _selectedDmgList = value;
                    RaisePropertyChanged("SelectedDmgList");
                }
            }
        }

        #endregion Properties

        #region Private Methods

        private void OnEnemyCalculatedMessage(EnemyCalculatedMessage obj)
        {
            _enemies = obj.Enemies;
        }

        private void OnCharakterSaveMessage(CharakterSaveMessage obj)
        {
            _charakters = obj.Charakters;
        }

        private void Parse(object obj)
        {
            if (_isLocked)
            {
                if (!string.IsNullOrEmpty(_skillFormula))
                {
                    IsValid = true;

                    _formulaCalculator.Formula = _skillFormula;
                    _formulaCalculator.Parse();

                    if (_formulaCalculator.HasParameters)
                    {
                        ValidateFormulaParameters();
                    }
                }
                else
                {
                    IsValid = false;

                    MessageBox.Show("Formula can not be empty!", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else
            {
                IsValid = false;

                MessageBox.Show("You have to lock the formula!", "Info", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void Calculate(object obj)
        {
            if (_charakters != null && _charakters.Count > 0 && _enemies != null && _enemies.Count > 0)
            {
                _results.Clear();

                foreach (CharakterClass charakter in _charakters)
                {
                    List<CharakterEnemyDmg> dmgList = _calculationService.CalculateDmg(_formulaCalculator, charakter,
                        _lvlTolerance, _enemies, _variance);

                    _results.Add(charakter.Level, dmgList);
                }

                SelectedCharakterIndex = -1;
                SelectedDmgList = null;
                RaisePropertyChanged("Results");
            }
            else
            {
                MessageBox.Show("You have to calculate an enemy first and enter charakter infos!", "Info",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void ValidateFormulaParameters()
        {
            foreach (string key in _formulaCalculator.Parameters.Keys)
            {
                if (key != Constants.AatkTag && key != Constants.AdefTag && key != Constants.AmatTag &&
                    key != Constants.AmdfTag && key != Constants.AmhpTag &&
                    key != Constants.AmmpTag
                    && key != Constants.BatkTag && key != Constants.BdefTag && key != Constants.BmatTag &&
                    key != Constants.BmdfTag && key != Constants.BmhpTag &&
                    key != Constants.BmmpTag)
                {
                    IsValid = false;

                    MessageBox.Show("You have invalid parameters in your skill formula!", "Warning",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
        }

        #endregion Private Methods
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Commands.Commands;
using ElzUtilLibary.Math.Formula;
using ElzUtilLibary.Math.Formula.Exceptions;
using ElzUtilLibary.Wpf.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using RpgEnemyLvlBalacingCalculator.Messages;
using RpgEnemyLvlBalacingCalculator.Model.Units;
using RpgEnemyLvlBalacingCalculator.Model.Util;
using RpgEnemyLvlBalacingCalculator.Services;
using RpgEnemyLvlBalacingCalculator.Util;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace RpgEnemyLvlBalacingCalculator.ViewModels
{
    public class EnemyTabViewModel : BaseViewModel
    {
        #region Members

        private readonly ICalculationService _calculationService;
        private readonly FormulaCalculator _formulaCalculator;
        private readonly IMessenger _messenger;
        private int _atk;
        private double _atkPer;
        private int _atkSet;
        private ICommand _calculateCommand;
        private int _def;
        private double _defPer;
        private int _defSet;
        private List<Enemy> _enemies;
        private string _enemyName;
        private string _formula = "base * (1.00 + (level-1) * per) + (set * (level-1))";
        private ObservableCollection<DictionaryClass> _formulaParameters;
        private bool _isLocked;
        private int _mat;
        private double _matPer;
        private int _matSet;
        private int _mdf;
        private double _mdfPer;
        private int _mdfSet;
        private int _mhp;
        private double _mhpPer;
        private int _mhpSet;
        private int _mmp;
        private double _mmpPer;
        private int _mmpSet;
        private ICommand _parseCommand;

        #endregion Members

        public EnemyTabViewModel(IMessenger messenger, ICalculationService calculationService)
        {
            _messenger = messenger;
            _calculationService = calculationService;
            _formulaCalculator = new FormulaCalculator();
        }

        #region Properties

        public string Formula
        {
            get { return _formula; }
            set
            {
                if (value != null && value != _formula)
                {
                    _formula = value;
                    RaisePropertyChanged("Formula");
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

        public ObservableCollection<DictionaryClass> FormulaParameters
        {
            get { return ConvertToObservable(_formulaCalculator.Parameters); }
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

        public int Mhp
        {
            get { return _mhp; }
            set
            {
                if (value != _mhp)
                {
                    _mhp = value;
                    RaisePropertyChanged("Mhp");
                }
            }
        }

        public int Mmp
        {
            get { return _mmp; }
            set
            {
                if (value != _mmp)
                {
                    _mmp = value;
                    RaisePropertyChanged("Mmp");
                }
            }
        }

        public int Atk
        {
            get { return _atk; }
            set
            {
                if (value != _atk)
                {
                    _atk = value;
                    RaisePropertyChanged("Atk");
                }
            }
        }

        public int Def
        {
            get { return _def; }
            set
            {
                if (value != _def)
                {
                    _def = value;
                    RaisePropertyChanged("Def");
                }
            }
        }

        public int Mat
        {
            get { return _mat; }
            set
            {
                if (value != _mat)
                {
                    _mat = value;
                    RaisePropertyChanged("Mat");
                }
            }
        }

        public int Mdf
        {
            get { return _mdf; }
            set
            {
                if (value != _mdf)
                {
                    _mdf = value;
                    RaisePropertyChanged("Mdf");
                }
            }
        }

        public double MhpPer
        {
            get { return _mhpPer; }
            set
            {
                _mhpPer = value;
                RaisePropertyChanged("MhpPer");
            }
        }

        public double MmpPer
        {
            get { return _mmpPer; }
            set
            {
                _mmpPer = value;
                RaisePropertyChanged("MmpPer");
            }
        }

        public double AtkPer
        {
            get { return _atkPer; }
            set
            {
                _atkPer = value;
                RaisePropertyChanged("AtkPer");
            }
        }

        public double DefPer
        {
            get { return _defPer; }
            set
            {
                _defPer = value;
                RaisePropertyChanged("DefPer");
            }
        }

        public double MatPer
        {
            get { return _matPer; }
            set
            {
                _matPer = value;
                RaisePropertyChanged("MatPer");
            }
        }

        public double MdfPer
        {
            get { return _mdfPer; }
            set
            {
                _mdfPer = value;
                RaisePropertyChanged("MdfPer");
            }
        }

        public int MhpSet
        {
            get { return _mhpSet; }
            set
            {
                if (value != _mhpSet)
                {
                    _mhpSet = value;
                    RaisePropertyChanged("MhpSet");
                }
            }
        }

        public int MmpSet
        {
            get { return _mmpSet; }
            set
            {
                if (value != _mmpSet)
                {
                    _mmpSet = value;
                    RaisePropertyChanged("MmpSet");
                }
            }
        }

        public int AtkSet
        {
            get { return _atkSet; }
            set
            {
                if (value != _atkSet)
                {
                    _atkSet = value;
                    RaisePropertyChanged("AtkSet");
                }
            }
        }

        public int DefSet
        {
            get { return _defSet; }
            set
            {
                if (value != _defSet)
                {
                    _defSet = value;
                    RaisePropertyChanged("DefSet");
                }
            }
        }

        public int MatSet
        {
            get { return _matSet; }
            set
            {
                if (value != _matSet)
                {
                    _matSet = value;
                    RaisePropertyChanged("MatSet");
                }
            }
        }

        public int MdfSet
        {
            get { return _mdfSet; }
            set
            {
                if (value != _mdfSet)
                {
                    _mdfSet = value;
                    RaisePropertyChanged("MdfSet");
                }
            }
        }

        public string EnemyName
        {
            get { return _enemyName; }
            set
            {
                if (value != null && value != _enemyName)
                {
                    _enemyName = value;
                    RaisePropertyChanged("EnemyName");
                }
            }
        }

        public List<Enemy> Enemies
        {
            get { return _enemies ?? (_enemies = new List<Enemy>()); }
            set
            {
                if (value != _enemies)
                {
                    _enemies = value;
                    RaisePropertyChanged("Enemies");
                }
            }
        }

        #endregion Properties

        #region Private Methods

        private ObservableCollection<DictionaryClass> ConvertToObservable(Dictionary<string, double?> parameters)
        {
            _formulaParameters = new ObservableCollection<DictionaryClass>();

            foreach (string key in parameters.Keys)
            {
                if (key != Constants.BaseTag && key != Constants.PerTag && key != Constants.SetTag)
                {
                    _formulaParameters.Add(new DictionaryClass
                    {
                        Key = key,
                        Value = parameters[key],
                    });
                }
            }

            return _formulaParameters;
        }

        private void Parse(object obj)
        {
            if (_isLocked)
            {
                if (!string.IsNullOrEmpty(_formula))
                {
                    _formulaCalculator.Formula = _formula;
                    _formulaCalculator.Parse();

                    if (_formulaCalculator.HasParameters)
                    {
                        RaisePropertyChanged("FormulaParameters");
                    }
                }
                else
                {
                    MessageBox.Show("Formula can not be empty!", "Info", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("You have to lock the formula!", "Info", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void Calculate(object obj)
        {
            Enemies.Clear();

            if (_formulaCalculator.Parameters.ContainsKey(Constants.LevelTag))
            {
                try
                {
                    object value = 0;

                    if (_formulaParameters.First(fp => (string) fp.Key == Constants.LevelTag).Value != null)
                    {
                        value = _formulaParameters.First(fp => (string) fp.Key == Constants.LevelTag).Value;
                    }

                    int maxLevel = int.Parse("" + value);

                    SetFormulaParametersWithoutBasePerSetAndLevel();

                    List<Enemy> enemies = Enemies;

                    for (int i = 1; i <= maxLevel; i++)
                    {
                        double mhp = _calculationService.CalculateStat(_formulaCalculator, _mhp, i, _mhpPer, _mhpSet);
                        double mmp = _calculationService.CalculateStat(_formulaCalculator, _mmp, i, _mmpPer, _mmpSet);
                        double atk = _calculationService.CalculateStat(_formulaCalculator, _atk, i, _atkPer, _atkSet);
                        double def = _calculationService.CalculateStat(_formulaCalculator, _def, i, _defPer, _defSet);
                        double mat = _calculationService.CalculateStat(_formulaCalculator, _mat, i, _matPer, _matSet);
                        double mdf = _calculationService.CalculateStat(_formulaCalculator, _mdf, i, _mdfPer, _mdfSet);

                        enemies.Add(new Enemy
                        {
                            Name = _enemyName,
                            Level = i,
                            Mhp = mhp,
                            Mmp = mmp,
                            Atk = atk,
                            Def = def,
                            Mat = mat,
                            Mdf = mdf,
                        });
                    }

                    Enemies = new List<Enemy>(enemies);

                    _messenger.Send(new EnemyCalculatedMessage(_enemies));
                }
                catch (FormulaParametersNotSetException)
                {
                    MessageBox.Show("You have to set all parameters of the formula!", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void SetFormulaParametersWithoutBasePerSetAndLevel()
        {
            foreach (DictionaryClass formulaParameter in _formulaParameters)
            {
                if ((string) formulaParameter.Key != Constants.BaseTag &&
                    (string) formulaParameter.Key != Constants.PerTag &&
                    (string) formulaParameter.Key != Constants.SetTag &&
                    (string) formulaParameter.Key != Constants.LevelTag)
                {
                    if (formulaParameter.Value != null)
                    {
                        _formulaCalculator.Parameters[(string) formulaParameter.Key] =
                            double.Parse("" + formulaParameter.Value);
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
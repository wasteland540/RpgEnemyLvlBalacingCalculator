using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using ElzUtilLibary.Math.Formula;
using RpgEnemyLvlBalacingCalculator.Model.Units;
using RpgEnemyLvlBalacingCalculator.Util;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace RpgEnemyLvlBalacingCalculator.Services
{
    public class CalculationService : ICalculationService
    {
        public double CalculateStat(FormulaCalculator formulaCalculator, int stat, int level, double per, int set)
        {
            formulaCalculator.Parameters[Constants.LevelTag] = level;
            formulaCalculator.Parameters[Constants.BaseTag] = stat;
            formulaCalculator.Parameters[Constants.PerTag] = per;
            formulaCalculator.Parameters[Constants.SetTag] = set;

            try
            {
                formulaCalculator.Calculate();
            }
            catch (SyntaxErrorException)
            {
                MessageBox.Show("You have a syntax error in the formula!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return formulaCalculator.Result;
        }

        public List<CharakterEnemyDmg> CalculateDmg(FormulaCalculator formulaCalculator, CharakterClass charakter,
            int lvlTolerance, List<Enemy> enemies, float variance)
        {
            var dmgList = new List<CharakterEnemyDmg>();

            int min = charakter.Level - lvlTolerance;
            int max = charakter.Level + lvlTolerance;

            if (min < 1)
            {
                min = 1;
            }

            if (max > enemies.Max(e => e.Level))
            {
                max = enemies.Max(e => e.Level);
            }

            for (int i = min; i <= max; i++)
            {
                Enemy enemy = enemies.FirstOrDefault(e => e.Level == i);

                if (enemy != null)
                {
                    double charakterMaxDmg = CalculateDmg(formulaCalculator, charakter, enemy, variance,
                        CharakterEnemyDmg.DamageCalculation.Max);
                    double charakterMinDmg = CalculateDmg(formulaCalculator, charakter, enemy, variance,
                        CharakterEnemyDmg.DamageCalculation.Min);
                    double enemyMaxDmg = CalculateDmg(formulaCalculator, enemy, charakter, variance,
                        CharakterEnemyDmg.DamageCalculation.Max);
                    double enemyMinDmg = CalculateDmg(formulaCalculator, enemy, charakter, variance,
                        CharakterEnemyDmg.DamageCalculation.Min);

                    dmgList.Add(new CharakterEnemyDmg
                    {
                        CharakterLevel = charakter.Level,
                        EnemyLevel = enemy.Level,
                        CharakterMaxDmg = charakterMaxDmg,
                        CharakterMinDmg = charakterMinDmg,
                        EnemyMaxDmg = enemyMaxDmg,
                        EnemyMinDmg = enemyMinDmg,
                    });
                }
            }

            return dmgList;
        }

        private double CalculateVarianceValue(double dmg, float variance)
        {
            double variancePer = variance/100.00;

            return dmg*variancePer;
        }

        private double CalculateDmg(FormulaCalculator formulaCalculator, object attacker, object defender,
            float variance,
            CharakterEnemyDmg.DamageCalculation type)
        {
            SetAttackerParameters(formulaCalculator, attacker);
            SetDeffenderParameters(formulaCalculator, defender);

            formulaCalculator.Calculate();
            double dmg = formulaCalculator.Result;

            #region Calculate Variance

            double varianceValue = CalculateVarianceValue(dmg, variance);

            if (type == CharakterEnemyDmg.DamageCalculation.Min)
            {
                dmg -= varianceValue;
            }
            else if (type == CharakterEnemyDmg.DamageCalculation.Max)
            {
                dmg += varianceValue;
            }

            if (dmg < 0)
            {
                dmg = 0;
            }

            #endregion Calculate Variance

            return dmg;
        }

        private void SetAttackerParameters(FormulaCalculator formulaCalculator, object attacker)
        {
            if (attacker is CharakterClass)
            {
                var charakter = (CharakterClass) attacker;

                #region MHP

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AmhpTag))
                {
                    double aMhp = charakter.Mhp + charakter.MhpAccessory + charakter.MhpBody + charakter.MhpHead +
                                  charakter.MhpShield + charakter.MhpWeapon;

                    formulaCalculator.Parameters[Constants.AmhpTag] = aMhp;
                }

                #endregion MHP

                #region MMP

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AmmpTag))
                {
                    double aMmp = charakter.Mmp + charakter.MmpAccessory + charakter.MmpBody + charakter.MmpHead +
                                  charakter.MmpShield + charakter.MmpWeapon;

                    formulaCalculator.Parameters[Constants.AmmpTag] = aMmp;
                }

                #endregion MHP

                #region ATK

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AatkTag))
                {
                    double aAtk = charakter.Atk + charakter.AtkAccessory + charakter.AtkBody + charakter.AtkHead +
                                  charakter.AtkShield + charakter.AtkWeapon;

                    formulaCalculator.Parameters[Constants.AatkTag] = aAtk;
                }

                #endregion ATK

                #region DEF

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AdefTag))
                {
                    double aDef = charakter.Def + charakter.DefAccessory + charakter.DefBody + charakter.DefHead +
                                  charakter.DefShield + charakter.DefWeapon;

                    formulaCalculator.Parameters[Constants.AdefTag] = aDef;
                }

                #endregion DEF

                #region MAT

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AmatTag))
                {
                    double aMat = charakter.Mat + charakter.MatAccessory + charakter.MatBody + charakter.MatHead +
                                  charakter.MatShield + charakter.MatWeapon;

                    formulaCalculator.Parameters[Constants.AmatTag] = aMat;
                }

                #endregion MAT

                #region MDF

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AmdfTag))
                {
                    double aMdf = charakter.Mdf + charakter.MdfAccessory + charakter.MdfBody + charakter.MdfHead +
                                  charakter.MdfShield + charakter.MdfWeapon;

                    formulaCalculator.Parameters[Constants.AmdfTag] = aMdf;
                }

                #endregion MDF
            }
            else if (attacker is Enemy)
            {
                var enemy = (Enemy) attacker;

                #region MHP

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AmhpTag))
                {
                    formulaCalculator.Parameters[Constants.AmhpTag] = enemy.Mhp;
                }

                #endregion MHP

                #region MMP

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AmmpTag))
                {
                    formulaCalculator.Parameters[Constants.AmmpTag] = enemy.Mmp;
                }

                #endregion MHP

                #region ATK

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AatkTag))
                {
                    formulaCalculator.Parameters[Constants.AatkTag] = enemy.Atk;
                }

                #endregion ATK

                #region DEF

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AdefTag))
                {
                    formulaCalculator.Parameters[Constants.AdefTag] = enemy.Def;
                }

                #endregion DEF

                #region MAT

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AmatTag))
                {
                    formulaCalculator.Parameters[Constants.AmatTag] = enemy.Mat;
                }

                #endregion MAT

                #region MDF

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.AmdfTag))
                {
                    formulaCalculator.Parameters[Constants.AmdfTag] = enemy.Mdf;
                }

                #endregion MDF
            }
        }

        private void SetDeffenderParameters(FormulaCalculator formulaCalculator, object deffender)
        {
            if (deffender is CharakterClass)
            {
                var charakter = (CharakterClass) deffender;

                #region MHP

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BmhpTag))
                {
                    double bMhp = charakter.Mhp + charakter.MhpAccessory + charakter.MhpBody + charakter.MhpHead +
                                  charakter.MhpShield + charakter.MhpWeapon;

                    formulaCalculator.Parameters[Constants.BmhpTag] = bMhp;
                }

                #endregion MHP

                #region MMP

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BmmpTag))
                {
                    double bMmp = charakter.Mmp + charakter.MmpAccessory + charakter.MmpBody + charakter.MmpHead +
                                  charakter.MmpShield + charakter.MmpWeapon;

                    formulaCalculator.Parameters[Constants.BmmpTag] = bMmp;
                }

                #endregion MHP

                #region ATK

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BatkTag))
                {
                    double bAtk = charakter.Atk + charakter.AtkAccessory + charakter.AtkBody + charakter.AtkHead +
                                  charakter.AtkShield + charakter.AtkWeapon;

                    formulaCalculator.Parameters[Constants.BatkTag] = bAtk;
                }

                #endregion ATK

                #region DEF

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BdefTag))
                {
                    double bDef = charakter.Def + charakter.DefAccessory + charakter.DefBody + charakter.DefHead +
                                  charakter.DefShield + charakter.DefWeapon;

                    formulaCalculator.Parameters[Constants.BdefTag] = bDef;
                }

                #endregion DEF

                #region MAT

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BmatTag))
                {
                    double bMat = charakter.Mat + charakter.MatAccessory + charakter.MatBody + charakter.MatHead +
                                  charakter.MatShield + charakter.MatWeapon;

                    formulaCalculator.Parameters[Constants.BmatTag] = bMat;
                }

                #endregion MAT

                #region MDF

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BmdfTag))
                {
                    double bMdf = charakter.Mdf + charakter.MdfAccessory + charakter.MdfBody + charakter.MdfHead +
                                  charakter.MdfShield + charakter.MdfWeapon;

                    formulaCalculator.Parameters[Constants.BmdfTag] = bMdf;
                }

                #endregion MDF
            }
            else if (deffender is Enemy)
            {
                var enemy = (Enemy) deffender;

                #region MHP

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BmhpTag))
                {
                    formulaCalculator.Parameters[Constants.BmhpTag] = enemy.Mhp;
                }

                #endregion MHP

                #region MMP

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BmmpTag))
                {
                    formulaCalculator.Parameters[Constants.BmmpTag] = enemy.Mmp;
                }

                #endregion MHP

                #region ATK

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BatkTag))
                {
                    formulaCalculator.Parameters[Constants.BatkTag] = enemy.Atk;
                }

                #endregion ATK

                #region DEF

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BdefTag))
                {
                    formulaCalculator.Parameters[Constants.BdefTag] = enemy.Def;
                }

                #endregion DEF

                #region MAT

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BmatTag))
                {
                    formulaCalculator.Parameters[Constants.BmatTag] = enemy.Mat;
                }

                #endregion MAT

                #region MDF

                if (formulaCalculator.Parameters.Keys.Any(k => k == Constants.BmdfTag))
                {
                    formulaCalculator.Parameters[Constants.BmdfTag] = enemy.Mdf;
                }

                #endregion MDF
            }
        }
    }
}
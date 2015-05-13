using System.Collections.Generic;
using ElzUtilLibary.Math.Formula;
using RpgEnemyLvlBalacingCalculator.Model.Units;

namespace RpgEnemyLvlBalacingCalculator.Services
{
    public interface ICalculationService
    {
        double CalculateStat(FormulaCalculator formulaCalculator, int stat, int level, double per, int set);

        List<CharakterEnemyDmg> CalculateDmg(FormulaCalculator formulaCalculator, CharakterClass charakter,
            int lvlTolerance, List<Enemy> enemies, float variance);
    }
}
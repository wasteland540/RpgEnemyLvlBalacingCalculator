using System.Collections.Generic;
using RpgEnemyLvlBalacingCalculator.Model.Units;

namespace RpgEnemyLvlBalacingCalculator.Messages
{
    public class EnemyCalculatedMessage
    {
        public List<Enemy> Enemies { get; private set; }

        public EnemyCalculatedMessage(List<Enemy> enemies)
        {
            Enemies = enemies;
        }
    }
}
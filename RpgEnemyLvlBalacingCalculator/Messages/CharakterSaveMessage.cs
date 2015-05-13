using System.Collections.Generic;
using RpgEnemyLvlBalacingCalculator.Model.Units;

namespace RpgEnemyLvlBalacingCalculator.Messages
{
    public class CharakterSaveMessage
    {
        public CharakterSaveMessage(List<CharakterClass> charakters)
        {
            Charakters = charakters;
        }

        public List<CharakterClass> Charakters { get; private set; }
    }
}
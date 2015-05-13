namespace RpgEnemyLvlBalacingCalculator.Model.Units
{
    public class CharakterEnemyDmg
    {
        public enum DamageCalculation
        {
            Min,
            Max,
        }

        public int CharakterLevel { get; set; }
        public int EnemyLevel { get; set; }
        public double CharakterMinDmg { get; set; }
        public double CharakterMaxDmg { get; set; }
        public double EnemyMinDmg { get; set; }
        public double EnemyMaxDmg { get; set; }
    }
}
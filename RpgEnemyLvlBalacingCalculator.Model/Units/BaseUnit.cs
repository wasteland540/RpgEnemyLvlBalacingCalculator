namespace RpgEnemyLvlBalacingCalculator.Model.Units
{
    public abstract class BaseUnit
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public double Mhp { get; set; }
        public double Mmp { get; set; }
        public double Atk { get; set; }
        public double Def { get; set; }
        public double Mat { get; set; }
        public double Mdf { get; set; }
    }
}
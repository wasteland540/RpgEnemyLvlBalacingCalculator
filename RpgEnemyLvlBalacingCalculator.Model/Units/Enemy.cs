namespace RpgEnemyLvlBalacingCalculator.Model.Units
{
    public class Enemy : BaseUnit
    {
        public double MhpPer { get; set; }
        public double MmpPer { get; set; }
        public double AtkPer { get; set; }
        public double DefPer { get; set; }
        public double MatPer { get; set; }
        public double MdfPer { get; set; }

        public double MhpSet { get; set; }
        public double MmpSet { get; set; }
        public double AtkSet { get; set; }
        public double DefSet { get; set; }
        public double MatSet { get; set; }
        public double MdfSet { get; set; }
    }
}